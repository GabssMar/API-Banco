using ProjetoDsin.Endpoints;

namespace ProjetoDsin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<BancoContext>();

            // Adiciona CORS com política liberada para seu front
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PermitirFrontend", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddAuthorization();

            // Removido AddDbContext porque o contexto já configura internamente

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("PermitirFrontend");
            app.UseAuthorization();

            app.RegistrarEndPointsBancos(); // Seus endpoints personalizados

            app.UseDeveloperExceptionPage(); // Mostrar exceções detalhadas

            app.Run();
        }
    }
}