using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using ProjetoDsin.Models;
using ProjetoDsin.DTOs;

namespace ProjetoDsin.Endpoints
{
    public static class EPBanco
    {
        public static void RegistrarEndPointsBancos(this IEndpointRouteBuilder rotas)
        {
            RouteGroupBuilder rotasUsuarios = rotas.MapGroup("/Usuarios");
            rotasUsuarios.MapPost("/seed", async (BancoContext dbContext, bool excluirExistentes = false) =>
            {
                Usuario user1 = new Usuario(0, "Leonardo", "leonardo.dsin@gmail.com", "1234", 1912, 308308);
                if (excluirExistentes)
                {
                    dbContext.Usuarios.RemoveRange(dbContext.Usuarios);
                }
                dbContext.Usuarios.AddRange([user1]);
                await dbContext.SaveChangesAsync();
            }
            );
            rotasUsuarios.MapGet("/", async (BancoContext dbContext) =>
            {
                var usuarios = await dbContext.Usuarios.OrderBy(a => a.Nome)
                .Select(a => new Usuario
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    Email = a.Email,
                    Senha = a.Senha,
                    CodigoAgente = a.CodigoAgente,
                    CodigoOrg = a.CodigoOrg
                })
                .ToListAsync();
                return TypedResults.Ok(usuarios);
            });
            rotasUsuarios.MapGet("/{Id}", async (BancoContext dbContext, int id) =>
            {
                var usuario = await dbContext.Usuarios.Select(a => new Usuario
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    Email = a.Email,
                    Senha = a.Senha,
                    CodigoAgente = a.CodigoAgente,
                    CodigoOrg = a.CodigoOrg
                }).FirstOrDefaultAsync(a => a .Id == id);
                if (usuario == null)
                {
                    return Results.NotFound();
                }
                return TypedResults.Ok(usuario);
            });
            rotasUsuarios.MapPost("/", async (BancoContext dbContext, Usuario usuario) =>
            {
                var novoUsuario = dbContext.Usuarios.Add(usuario);
                await dbContext.SaveChangesAsync();
                return TypedResults.Created($"/usuarios/{usuario.Id}", usuario);
            });
            rotasUsuarios.MapPost("/login", async (BancoContext dbContext, Usuario login) =>
            {
                var usuario = await dbContext.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == login.Email && u.Senha == login.Senha);

                if (usuario == null)
                {
                    return Results.Unauthorized();
                }

                return TypedResults.Ok(new
                {
                    mensagem = "Login realizado com sucesso!",
                    usuario = new
                    {
                        usuario.Id,
                        usuario.Nome,
                        usuario.Email,
                        usuario.CodigoAgente,
                        usuario.CodigoOrg
                    }
                });
            });

            rotasUsuarios.MapGet("/me", async (HttpContext http, BancoContext dbContext) =>
            {
                string email = http.Request.Query["email"];

                var usuario = await dbContext.Usuarios
                    .Select(a => new
                    {
                        a.Id,
                        a.Nome,
                        a.Email,
                        a.CodigoAgente,
                        a.CodigoOrg
                    })
                    .FirstOrDefaultAsync(a => a.Email == email);

                if (usuario == null)
                    return Results.NotFound();

                return Results.Ok(usuario);
            });


            rotas.MapPost("/multas", async (BancoContext db, MultaDTO dto) =>
            {
                var veiculo = new DadosVeiculo
                {
                    Placa = dto.DadosVeiculo.Placa,
                    Modelo = dto.DadosVeiculo.Modelo,
                    Fabricante = dto.DadosVeiculo.Fabricante,
                    Cor = dto.DadosVeiculo.Cor,
                    Ano = dto.DadosVeiculo.Ano,
                    IdUsuario = dto.DadosVeiculo.IdUsuario
                };

                db.DadosVeiculos.Add(veiculo);
                await db.SaveChangesAsync();

                var proprietario = new DadosProprietario
                {
                    Nome = dto.DadosProprietario.Nome,
                    Cnh = dto.DadosProprietario.Cnh,
                    Cpf = dto.DadosProprietario.Cpf,
                    DadosVeiculo = veiculo,
                    IdDadosVeiculo = veiculo.Id
                };

                var infracao = new DetalhesInfracao
                {
                    TipoInfracao = dto.DetalhesInfracao.TipoInfracao,
                    CodigoInfracao = dto.DetalhesInfracao.CodigoInfracao,
                    LocaInfracao = dto.DetalhesInfracao.LocaInfracao,
                    Data = dto.DetalhesInfracao.Data,
                    Hota = dto.DetalhesInfracao.Hota,
                    Gravidade = dto.DetalhesInfracao.Gravidade,
                    PontosCnh = dto.DetalhesInfracao.PontosCnh,
                    DadosVeiculo = veiculo,
                    IdDadosVeiculo = veiculo.Id
                };

                var anexo = new Anexos
                {
                    Evidencia = dto.Anexos.Evidencia,
                    Comentarios = dto.Anexos.Comentarios,
                    DadosVeiculo = veiculo,
                    IdDadosVeiculo = veiculo.Id
                };

                db.DadosProprietarios.Add(proprietario);
                db.DetalhesInfracaos.Add(infracao);
                db.Anexos.Add(anexo);

                await db.SaveChangesAsync();

                return Results.Ok(new { mensagem = "Multa cadastrada com sucesso" });
            });

            rotas.MapGet("/multas", async (BancoContext db) =>
            {
                var multas = await db.DadosVeiculos
                    .Select(v => new
                    {
                        v.Id,
                        v.Placa,
                        v.Modelo,
                        v.Fabricante,
                        v.Cor,
                        v.Ano,

                        Proprietario = db.DadosProprietarios
                            .Where(p => p.IdDadosVeiculo == v.Id)
                            .Select(p => new
                            {
                                p.Nome,
                                p.Cnh,
                                p.Cpf
                            })
                            .FirstOrDefault(),

                        DetalhesInfracao = db.DetalhesInfracaos
                            .Where(i => i.IdDadosVeiculo == v.Id)
                            .Select(i => new
                            {
                                i.TipoInfracao,
                                i.CodigoInfracao,
                                LocalInfracao = i.LocaInfracao,
                                i.Data,
                                Hora = i.Hota,
                                i.Gravidade,
                                i.PontosCnh
                            })
                            .FirstOrDefault(),

                        Anexos = db.Anexos
                            .Where(a => a.IdDadosVeiculo == v.Id)
                            .Select(a => new
                            {
                                a.Evidencia,
                                a.Comentarios
                            })
                            .FirstOrDefault()
                    })
                    .ToListAsync();

                return Results.Ok(multas);
            });

            rotas.MapDelete("/multas/{id}", async (BancoContext db, int id) =>
            {
                var veiculo = await db.DadosVeiculos.FindAsync(id);

                if (veiculo == null)
                {
                    return Results.NotFound(new { mensagem = "Multa não encontrada." });
                }

                // Buscar e remover o proprietário associado
                var proprietario = await db.DadosProprietarios.FirstOrDefaultAsync(p => p.IdDadosVeiculo == id);
                if (proprietario != null)
                {
                    db.DadosProprietarios.Remove(proprietario);
                }

                // Buscar e remover os detalhes da infração associados
                var infracao = await db.DetalhesInfracaos.FirstOrDefaultAsync(i => i.IdDadosVeiculo == id);
                if (infracao != null)
                {
                    db.DetalhesInfracaos.Remove(infracao);
                }

                // Buscar e remover os anexos associados
                var anexo = await db.Anexos.FirstOrDefaultAsync(a => a.IdDadosVeiculo == id);
                if (anexo != null)
                {
                    db.Anexos.Remove(anexo);
                }

                // Por fim, remover o veículo
                db.DadosVeiculos.Remove(veiculo);

                await db.SaveChangesAsync();

                return Results.Ok(new { mensagem = "Multa excluída com sucesso." });
            });


        }
    }
}
