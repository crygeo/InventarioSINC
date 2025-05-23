using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Servidor.src.Extensiones;
using Servidor.src.Helper;
using Servidor.src.Hubs;
using Servidor.src.HubsService;
using Servidor.src.Objs;
using Servidor.src.Repositorios;
using Servidor.src.Services;
using System;
using System.Linq;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//var secretKey = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key not found"));
var secretKey = Encoding.UTF8.GetBytes("CryGeoElProgramadorDelSigloFracasadoPipipi");

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenLocalhost(5000); // HTTP
    serverOptions.ListenLocalhost(5001, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS con certificado de desarrollo
    });
});


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();


builder.Services.AddScoped<HubsServiceUsuario>();
builder.Services.AddScoped<HubsServiceRol>();
builder.Services.AddScoped<HubsServiceProveedor>();
builder.Services.AddScoped<HubsServiceTalla>();
builder.Services.AddScoped<HubsServiceRecepcionCarga>();
builder.Services.AddScoped<HubsServiceClasificacion>();
builder.Services.AddScoped<HubsServiceClase>();
builder.Services.AddScoped<HubsServiceCalidad>();

// Configurar la inyección de dependencias para MongoDB y tus repositorios
builder.Services.AddScoped<ServiceUsuario>();
builder.Services.AddScoped<ServiceRol>();
builder.Services.AddScoped<ServiceProveedor>();
builder.Services.AddScoped<ServiceTalla>();
builder.Services.AddScoped<ServiceRecepcionCarga>();
builder.Services.AddScoped<ServiceClasificacion>();
builder.Services.AddScoped<ServiceClase>();
builder.Services.AddScoped<ServiceCalidad>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()); // <-- Permitir Authorization en los headers
});

// 🔹 Configurar JWT
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // No permite tolerancia en la expiración del token
        };
    });




var app = builder.Build();


app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();  // <-- JWT Authentication
app.UseAuthorization();   // <-- Authorization

// Middleware para capturar headers (después de autorización)
app.Use(async (context, next) =>
{
    var token = context.Request.Headers["Authorization"].FirstOrDefault();

    if (!string.IsNullOrEmpty(token))
    {
        var Usuario = GenerateTokenForUser.ValidateToken(token.Replace("Bearer ", ""));
        if (Usuario != null)
        {
            context.User = Usuario;
        }
    }

    await next();
});

app.MapControllers();
app.MapHubByConvention<HubUsuario>();
app.MapHubByConvention<HubRol>();
app.MapHubByConvention<HubProveedor>();
app.MapHubByConvention<HubTalla>();
app.MapHubByConvention<HubRecepcionCarga>();
app.MapHubByConvention<HubClasificacion>();
app.MapHubByConvention<HubClase>();
app.MapHubByConvention<HubCalidad>();

app.Run();


