
using ecommerce_iniciativatena.Data;
using ecommerce_iniciativatena.Model;
using ecommerce_iniciativatena.Security.Implements;
using ecommerce_iniciativatena.Security;
using ecommerce_iniciativatena.Service;
using ecommerce_iniciativatena.Service.Implements;
using ecommerce_iniciativatena.Validator;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ecommerce_iniciativatena.Configuration;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

namespace ecommerce_iniciativatena
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                }
            );

            // Conex�o com o Banco de dados
            if (builder.Configuration["Environment:Start"] == "PROD")
            {
                // Conex�o com o PostgresSQL - Nuvem

                builder.Configuration
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("secrets.json");

                var connectionString = builder.Configuration
               .GetConnectionString("ProdConnection");

                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(connectionString)
                );
            }
            else
            {
                // Conex�o com o SQL Server - Localhost
                var connectionString = builder.Configuration
                .GetConnectionString("DefaultConnection");

                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString)
                );
            }

            // Valida��o de Entidades
            builder.Services.AddTransient<IValidator<Categoria>, CategoriaValidator>();
            builder.Services.AddTransient<IValidator<Produto>, ProdutoValidator>();
            builder.Services.AddTransient<IValidator<User>, UserValidator>();


            // Registrar as Classes e Interfaces Service
            builder.Services.AddScoped<ICategoriaService, CategoriaService>();
            builder.Services.AddScoped<IProdutoService, ProdutoService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddTransient<IAuthService, AuthService>();

            // Adicionar a Valida��o do Token JWT

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var Key = Encoding.UTF8.GetBytes(Settings.Secret);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            //Registrar o Swagger
            builder.Services.AddSwaggerGen(options =>
            {

                //Personalizar a P�gna inicial do Swagger
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Iniciativa Atena",
                    Description = "Projeto Integrador Iniciativa Atena - ASP.NET Core 7 - Entity Framework",
                    Contact = new OpenApiContact
                    {
                        Name = "Iniciativa Atena",
                        Email = "projetointegrador03gen@gmail.com",
                        Url = new Uri("https://github.com/Projeto-Integrador-Grupo03")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Github",
                        Url = new Uri("https://github.com/Projeto-Integrador-Grupo03")
                    }
                });

                //Adicionar a Seguran�a no Swagger
                options.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Digite um Token JWT v�lido!",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                //Adicionar a configura��o visual da Seguran�a no Swagger
                options.OperationFilter<AuthResponsesOperationFilter>();

            });

            // Adicionar o Fluent Validation no Swagger
            builder.Services.AddFluentValidationRulesToSwagger();

            // Configuração do CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "MyPolicy",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });


            var app = builder.Build();

            // Criar o Banco de dados e as tabelas Automaticamente
            using (var scope = app.Services.CreateAsyncScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
            }

            app.UseDeveloperExceptionPage();

            // Configure the HTTP request pipeline.

            
                app.UseSwagger();
                app.UseSwaggerUI();

            // Swagger Como P�gina Inicial - Nuvem

            if (app.Environment.IsProduction())
            {
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Iniciativa Atena - v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            //Habilitar CORS
            app.UseCors("MyPolicy");

            // Habilitar a Autentica��o e a Autoriza��o
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}