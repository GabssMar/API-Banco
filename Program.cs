
using ProjetoDsin.Endpoints;

namespace ProjetoDsin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adiciona CORS com política liberada para seu front
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PermitirFrontend", policy =>
                {
                    policy.AllowAnyOrigin() // Coloque aqui o endereço do seu front
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            

            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<BancoContext>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Habilita CORS para as rotas
            app.UseCors("PermitirFrontend");

            app.UseAuthorization();

            app.RegistrarEndPointsBancos();

            app.UseDeveloperExceptionPage();

            app.Run();
        }
    }
}
