using BarcodeAPI.Entities;
using BarcodeAPI.Middleware;
using BarcodeAPI.Models;
using BarcodeAPI.Models.Validation;
using BarcodeAPI.Services;
using NLog.Web;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BarcodeAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var appSettings = new AppSettings();

        builder.Configuration.GetSection("Auth").Bind(appSettings);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Db
        builder.Services.AddScoped<Seeder>();
        builder.Services.AddScoped<ProductsDbContext>();

        //Services
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IAccountService, AccountService>();

        //Helpery
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Host.UseNLog();
        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        builder.Services.AddSingleton(appSettings);

        //Validation
        //Dodanie FluentValidation
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddScoped<IValidator<AddUserBodyRequest>, AddUserRequestBodyValidator>();

        //JWT
        builder.Services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = "Bearer";
            option.DefaultScheme = "Bearer";
            option.DefaultChallengeScheme = "Bearer";
        }).AddJwtBearer(cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = appSettings.JwtIssuer,
                ValidAudience = appSettings.JwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JwtKey)),
            };
        });

        var app = builder.Build();

        /*Dodanie ErrorHandlingMiddleware DONE
         *Nloga DONE
         *Stworzenie DB
         *Dodanie Kontrolera dla account
         *Dodanie mo¿liwoœci rejestracji
         *Dodanie mo¿liwoœci logowania
         *Dodanie JWT
         */

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseAuthentication();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseSwagger();

        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Barcode API"));

        app.MapControllers();

        SeedDatabase();

        app.Run();

        void SeedDatabase() //can be placed at the very bottom under app.Run()
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<Seeder>();
                dbInitializer.Seed();
            }
        }
    }
}