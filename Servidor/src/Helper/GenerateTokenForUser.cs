using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Servidor.Model;

namespace Servidor.Helper;

public static class GenerateTokenForUser
{
    private static readonly string
        SecretKey = "ClaveSuperSeguraDesdeConfiguracion"; // Cárgala desde appsettings.json o variables de entorno

    private static readonly byte[] Key = Encoding.UTF8.GetBytes(SecretKey);

    public static string GenerateToken(Usuario usuario, int expireMinutes = 60)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = GetClaims(usuario, expireMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        // Agregar los roles del usuario al token como claims adicionales
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null; // Token inválido
        }
    }

    private static List<Claim> GetClaims(Usuario usuario, int expireMinutes)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id),
            new(ClaimTypes.Name, usuario.User),
            new(ClaimTypes.Dns, usuario.Cedula),
            new(ClaimTypes.MobilePhone, usuario.Celular),
            new("exp",
                DateTimeOffset.UtcNow.AddMinutes(expireMinutes).ToUnixTimeSeconds()
                    .ToString()) // Agrega el tiempo de expiración
        };

        // Agregar roles
        foreach (var rol in usuario.Roles) claims.Add(new Claim(ClaimTypes.Role, rol));

        return claims;
    }
}