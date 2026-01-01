using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Servidor;
using Servidor.Extensiones;
using Servidor.Helper;
using Servidor.Hubs;
using Shared.Interfaces.Model;

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


builder.Services.AddScoped<AppInitializer>();


var app = builder.Build();


// ✅ Asignar el service provider global al HubFactory
HubFactory.ServiceProvider = app.Services;

// Inicialización con scope
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<AppInitializer>();
    await initializer.InitAsync();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAllOrigins");
app.UseAuthentication(); // <-- JWT Authentication
app.UseAuthorization(); // <-- Authorization

// Middleware para capturar headers (después de autorización)
app.Use(async (context, next) =>
{
    var token = context.Request.Headers["Authorization"].FirstOrDefault();

    if (!string.IsNullOrEmpty(token))
    {
        var Usuario = GenerateTokenForUser.ValidateToken(token.Replace("Bearer ", ""));
        if (Usuario != null) context.User = Usuario;
    }

    await next();
});

app.MapControllers();

app.MapAllGenericHubs<IModelObj>();

app.Run();