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
                // Corrigido aqui: usar Add em vez de AddRange com colchetes inválidos
                dbContext.Usuarios.Add(user1);
                await dbContext.SaveChangesAsync();
            });

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
                }).FirstOrDefaultAsync(a => a.Id == id);

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

            rotas.MapPost("/multas", async (HttpRequest request, BancoContext db) =>
            {
                var form = await request.ReadFormAsync();

                // Dados do veículo
                var placa = form["placa"];
                if (string.IsNullOrEmpty(placa)) placa = form["Placa"];
                var modelo = form["modelo"];
                if (string.IsNullOrEmpty(modelo)) modelo = form["Modelo"];
                var fabricante = form["fabricante"];
                if (string.IsNullOrEmpty(fabricante)) fabricante = form["Fabricante"];
                var cor = form["cor"];
                if (string.IsNullOrEmpty(cor))
                {
                    var dadosVeiculoJson = form["DadosVeiculo"];
                    Console.WriteLine($"Conteúdo bruto de DadosVeiculo: {dadosVeiculoJson}");
                    if (!string.IsNullOrEmpty(dadosVeiculoJson))
                    {
                        try
                        {
                            using var doc = System.Text.Json.JsonDocument.Parse(dadosVeiculoJson);
                            if (string.IsNullOrEmpty(placa) && doc.RootElement.TryGetProperty("Placa", out var placaElement))
                                placa = placaElement.GetString() ?? placaElement.GetRawText();
                            if (string.IsNullOrEmpty(modelo) && doc.RootElement.TryGetProperty("Modelo", out var modeloElement))
                                modelo = modeloElement.GetString() ?? modeloElement.GetRawText();
                            if (string.IsNullOrEmpty(fabricante) && doc.RootElement.TryGetProperty("Fabricante", out var fabricanteElement))
                                fabricante = fabricanteElement.GetString() ?? fabricanteElement.GetRawText();
                            if (string.IsNullOrEmpty(cor) && doc.RootElement.TryGetProperty("Cor", out var corElement))
                                cor = corElement.GetString() ?? corElement.GetRawText();
                            Console.WriteLine($"Placa extraída de DadosVeiculo (manual): '{placa}'");
                            Console.WriteLine($"Modelo extraído de DadosVeiculo (manual): '{modelo}'");
                            Console.WriteLine($"Fabricante extraído de DadosVeiculo (manual): '{fabricante}'");
                            Console.WriteLine($"Cor extraída de DadosVeiculo (manual): '{cor}'");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao ler DadosVeiculo manualmente: {ex.Message}");
                        }
                    }
                }
                Console.WriteLine($"Valor recebido para cor: '{cor}'");
                if (string.IsNullOrEmpty(cor))
                {
                    return Results.BadRequest(new { mensagem = "Campo 'cor' do veículo não informado." });
                }
                var anoStr = form["ano"].ToString();
                Console.WriteLine($"form[\"ano\"]: '{form["ano"]}'");
                Console.WriteLine($"form[\"DadosVeiculo\"]: '{form["DadosVeiculo"]}'");
                foreach (var key in form.Keys)
                {
                    Console.WriteLine($"Campo recebido: {key} = {form[key]}");
                }
                foreach (var file in form.Files)
                {
                    Console.WriteLine($"Arquivo recebido: {file.Name} ({file.FileName})");
                }
                if (string.IsNullOrEmpty(anoStr))
                {
                    var dadosVeiculoJson = form["DadosVeiculo"];
                    if (!string.IsNullOrEmpty(dadosVeiculoJson))
                    {
                        try
                        {
                            using var doc = System.Text.Json.JsonDocument.Parse(dadosVeiculoJson);
                            if (doc.RootElement.TryGetProperty("Ano", out var anoElement))
                            {
                                anoStr = anoElement.GetString() ?? anoElement.GetRawText();
                                Console.WriteLine($"Ano extraído de DadosVeiculo (manual): '{anoStr}'");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao ler Ano manualmente de DadosVeiculo: {ex.Message}");
                        }
                    }
                }
                anoStr = anoStr?.Trim('"', ' ', '\''); // Remove aspas duplas, simples e espaços
                Console.WriteLine($"Ano final para conversão: '{anoStr}'");
                int ano = 0;
                if (!int.TryParse(anoStr, out ano))
                {
                    return Results.BadRequest(new { mensagem = $"Ano inválido ou não informado. Valor recebido: '{anoStr}'" });
                }
                // idUsuario
                var idUsuarioStr = form["idUsuario"];
                if (string.IsNullOrEmpty(idUsuarioStr))
                {
                    var dadosVeiculoJson = form["DadosVeiculo"];
                    if (!string.IsNullOrEmpty(dadosVeiculoJson))
                    {
                        try
                        {
                            using var doc = System.Text.Json.JsonDocument.Parse(dadosVeiculoJson);
                            if (doc.RootElement.TryGetProperty("IdUsuario", out var idUsuarioElement))
                            {
                                if (idUsuarioElement.ValueKind == System.Text.Json.JsonValueKind.Number)
                                    idUsuarioStr = idUsuarioElement.GetRawText();
                                else
                                    idUsuarioStr = idUsuarioElement.GetString() ?? idUsuarioElement.GetRawText();
                                Console.WriteLine($"IdUsuario extraído de DadosVeiculo (manual): '{idUsuarioStr}'");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao ler IdUsuario manualmente de DadosVeiculo: {ex.Message}");
                        }
                    }
                }
                idUsuarioStr = idUsuarioStr.ToString().Trim('"', ' ', '\'');
                Console.WriteLine($"IdUsuario final para conversão: '{idUsuarioStr}'");
                if (!int.TryParse(idUsuarioStr, out var idUsuario))
                {
                    return Results.BadRequest(new { mensagem = $"ID do usuário inválido ou não informado. Valor recebido: '{idUsuarioStr}'" });
                }

                // pontosCnh
                var pontosCnhStr = form["pontosCnh"];
                if (string.IsNullOrEmpty(pontosCnhStr))
                {
                    var detalhesInfracaoJson = form["DetalhesInfracao"];
                    if (!string.IsNullOrEmpty(detalhesInfracaoJson))
                    {
                        try
                        {
                            using var doc = System.Text.Json.JsonDocument.Parse(detalhesInfracaoJson);
                            if (doc.RootElement.TryGetProperty("PontosCnh", out var pontosElement))
                            {
                                pontosCnhStr = pontosElement.GetString() ?? pontosElement.GetRawText();
                                Console.WriteLine($"PontosCnh extraído de DetalhesInfracao (manual): '{pontosCnhStr}'");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao ler PontosCnh manualmente de DetalhesInfracao: {ex.Message}");
                        }
                    }
                }
                pontosCnhStr = pontosCnhStr.ToString().Trim('"', ' ', '\'');
                Console.WriteLine($"PontosCnh final para conversão: '{pontosCnhStr}'");
                if (!int.TryParse(pontosCnhStr, out var pontosCnh))
                {
                    return Results.BadRequest(new { mensagem = $"Pontos na CNH inválidos ou não informados. Valor recebido: '{pontosCnhStr}'" });
                }

                // Dados do proprietário
                // Remover extração dos campos do proprietário
                // var nome = form["nome"];
                // if (string.IsNullOrEmpty(nome)) nome = form["Nome"];
                // var cnh = form["cnh"];
                // if (string.IsNullOrEmpty(cnh)) cnh = form["Cnh"];
                // var cpf = form["cpf"];
                // if (string.IsNullOrEmpty(cpf)) cpf = form["Cpf"];

                // Dados da infração
                var tipoInfracao = form["tipoInfracao"];
                if (string.IsNullOrEmpty(tipoInfracao)) tipoInfracao = form["TipoInfracao"];
                if (string.IsNullOrEmpty(tipoInfracao))
                {
                    var detalhesInfracaoJson = form["DetalhesInfracao"];
                    if (!string.IsNullOrEmpty(detalhesInfracaoJson))
                    {
                        try
                        {
                            using var doc = System.Text.Json.JsonDocument.Parse(detalhesInfracaoJson);
                            if (doc.RootElement.TryGetProperty("TipoInfracao", out var tipoElement))
                            {
                                tipoInfracao = tipoElement.GetString() ?? tipoElement.GetRawText();
                                Console.WriteLine($"TipoInfracao extraído de DetalhesInfracao (manual): '{tipoInfracao}'");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao ler TipoInfracao manualmente de DetalhesInfracao: {ex.Message}");
                        }
                    }
                }
                if (string.IsNullOrEmpty(tipoInfracao))
                {
                    return Results.BadRequest(new { mensagem = "Campo 'tipoInfracao' da infração não informado." });
                }
                var localInfracao = form["localInfracao"];
                if (string.IsNullOrEmpty(localInfracao)) localInfracao = form["LocalInfracao"];
                if (string.IsNullOrEmpty(localInfracao))
                {
                    var detalhesInfracaoJson = form["DetalhesInfracao"];
                    if (!string.IsNullOrEmpty(detalhesInfracaoJson))
                    {
                        try
                        {
                            using var doc = System.Text.Json.JsonDocument.Parse(detalhesInfracaoJson);
                            if (doc.RootElement.TryGetProperty("LocalInfracao", out var localElement))
                            {
                                localInfracao = localElement.GetString() ?? localElement.GetRawText();
                                Console.WriteLine($"LocalInfracao extraído de DetalhesInfracao (manual): '{localInfracao}'");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao ler LocalInfracao manualmente de DetalhesInfracao: {ex.Message}");
                        }
                    }
                }
                if (string.IsNullOrEmpty(localInfracao))
                {
                    return Results.BadRequest(new { mensagem = "Campo 'localInfracao' da infração não informado." });
                }
                var dataStr = form["data"];
                if (string.IsNullOrEmpty(dataStr))
                {
                    var detalhesInfracaoJson = form["DetalhesInfracao"];
                    if (!string.IsNullOrEmpty(detalhesInfracaoJson))
                    {
                        try
                        {
                            using var doc = System.Text.Json.JsonDocument.Parse(detalhesInfracaoJson);
                            if (doc.RootElement.TryGetProperty("Data", out var dataElement))
                            {
                                dataStr = dataElement.GetString() ?? dataElement.GetRawText();
                                Console.WriteLine($"Data extraída de DetalhesInfracao (manual): '{dataStr}'");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao ler Data manualmente de DetalhesInfracao: {ex.Message}");
                        }
                    }
                }
                dataStr = dataStr.ToString(); // Garante que é string ANTES de qualquer uso
                var dataStrTrimmed = dataStr.ToString().Trim(new char[] {'"', ' ', '\''});
                Console.WriteLine($"Data final para conversão: '{dataStrTrimmed}'");
                DateTime data;
                if (!DateTime.TryParse(dataStrTrimmed, out data))
                {
                    // Tenta converter manualmente do formato ddMMyyyy
                    if (dataStrTrimmed.Length == 8 &&
                        int.TryParse(dataStrTrimmed.Substring(0, 2), out int dia) &&
                        int.TryParse(dataStrTrimmed.Substring(2, 2), out int mes) &&
                        int.TryParse(dataStrTrimmed.Substring(4, 4), out int anoManual))
                    {
                        try
                        {
                            data = new DateTime(anoManual, mes, dia);
                        }
                        catch
                        {
                            return Results.BadRequest(new { mensagem = $"Data inválida ou não informada. Valor recebido: '{dataStrTrimmed}'" });
                        }
                    }
                    else
                    {
                        return Results.BadRequest(new { mensagem = $"Data inválida ou não informada. Valor recebido: '{dataStrTrimmed}'" });
                    }
                }
                var hora = form["hora"];
                if (string.IsNullOrEmpty(hora)) hora = form["Hora"];
                if (string.IsNullOrEmpty(hora))
                {
                    var detalhesInfracaoJson = form["DetalhesInfracao"];
                    if (!string.IsNullOrEmpty(detalhesInfracaoJson))
                    {
                        try
                        {
                            using var doc = System.Text.Json.JsonDocument.Parse(detalhesInfracaoJson);
                            if (doc.RootElement.TryGetProperty("Hora", out var horaElement))
                            {
                                hora = horaElement.GetString() ?? horaElement.GetRawText();
                                Console.WriteLine($"Hora extraída de DetalhesInfracao (manual): '{hora}'");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao ler Hora manualmente de DetalhesInfracao: {ex.Message}");
                        }
                    }
                }
                if (string.IsNullOrEmpty(hora))
                {
                    return Results.BadRequest(new { mensagem = "Campo 'hora' da infração não informado." });
                }
                var gravidade = form["gravidade"];
                if (string.IsNullOrEmpty(gravidade)) gravidade = form["Gravidade"];
                if (string.IsNullOrEmpty(gravidade))
                {
                    var detalhesInfracaoJson = form["DetalhesInfracao"];
                    if (!string.IsNullOrEmpty(detalhesInfracaoJson))
                    {
                        try
                        {
                            using var doc = System.Text.Json.JsonDocument.Parse(detalhesInfracaoJson);
                            if (doc.RootElement.TryGetProperty("Gravidade", out var gravidadeElement))
                            {
                                gravidade = gravidadeElement.GetString() ?? gravidadeElement.GetRawText();
                                Console.WriteLine($"Gravidade extraída de DetalhesInfracao (manual): '{gravidade}'");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao ler Gravidade manualmente de DetalhesInfracao: {ex.Message}");
                        }
                    }
                }
                if (string.IsNullOrEmpty(gravidade))
                {
                    return Results.BadRequest(new { mensagem = "Campo 'gravidade' da infração não informado." });
                }

                // Comentários
                var comentarios = form["comentarios"];
                if (string.IsNullOrEmpty(comentarios)) comentarios = form["Comentarios"];

                // Imagem recebida
                var imagem = form.Files["evidencia"] ?? form.Files["Evidencia"];
                if (imagem == null)
                {
                    return Results.BadRequest(new { mensagem = "Arquivo de evidência não enviado." });
                }
                byte[] imagemBytes;
                using (var ms = new MemoryStream())
                {
                    await imagem.CopyToAsync(ms);
                    imagemBytes = ms.ToArray();
                }

                var veiculo = new DadosVeiculo
                {
                    Placa = placa,
                    Modelo = modelo,
                    Fabricante = fabricante,
                    Cor = cor,
                    Ano = ano,
                    IdUsuario = idUsuario
                };
                db.DadosVeiculos.Add(veiculo);
                await db.SaveChangesAsync();

                var proprietario = new DadosProprietario
                {
                    IdDadosVeiculo = veiculo.Id,
                    DadosVeiculo = veiculo
                };
                db.DadosProprietarios.Add(proprietario);

                var infracao = new DetalhesInfracao
                {
                    TipoInfracao = tipoInfracao,
                    LocalInfracao = localInfracao,  // corrigido
                    Data = dataStrTrimmed,
                    Hora = hora,                    // corrigido
                    Gravidade = gravidade,
                    PontosCnh = pontosCnh,
                    IdDadosVeiculo = veiculo.Id,
                    DadosVeiculo = veiculo
                };
                db.DetalhesInfracaos.Add(infracao);

                var anexo = new Anexos
                {
                    Evidencia = imagemBytes,
                    Comentarios = comentarios,
                    IdDadosVeiculo = veiculo.Id,
                    DadosVeiculo = veiculo
                };
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
                                p.Id,
                                p.IdDadosVeiculo
                            })
                            .FirstOrDefault(),

                        DetalhesInfracao = db.DetalhesInfracaos
                            .Where(i => i.IdDadosVeiculo == v.Id)
                            .Select(i => new
                            {
                                i.TipoInfracao,
                                i.CodigoInfracao,
                                LocalInfracao = i.LocalInfracao,  // corrigido
                                i.Data,
                                Hora = i.Hora,                    // corrigido
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
