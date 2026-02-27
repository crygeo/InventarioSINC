using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Servidor.Model;

namespace Servidor.Helper;

/// <summary>
/// Wrapper simple para transportar la clave JWT via DI.
/// </summary>
public class JwtSecretKey
{
    public byte[] Key { get; }
    public JwtSecretKey(byte[] key) => Key = key;
}

public static class GenerateTokenForUser
{
    private static byte[] _key = Array.Empty<byte>();

    /// <summary>
    /// Debe llamarse una sola vez al arrancar la aplicación (en Program.cs).
    /// </summary>
    public static void Initialize(byte[] key)
    {
        _key = key;
    }

    public static string GenerateToken(Usuario usuario, int expireMinutes = 60)
    {
        EnsureInitialized();

        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = GetClaims(usuario, expireMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static ClaimsPrincipal? ValidateToken(string token)
    {
        EnsureInitialized();

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(_key),
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
                DateTimeOffset.UtcNow.AddMinutes(expireMinutes)
                    .ToUnixTimeSeconds().ToString())
        };

        foreach (var rol in usuario.Roles)
            claims.Add(new Claim(ClaimTypes.Role, rol));

        return claims;
    }

    private static void EnsureInitialized()
    {
        if (_key.Length == 0)
            throw new InvalidOperationException(
                "GenerateTokenForUser no fue inicializado. Llama a Initialize() al arrancar la aplicación.");
    }
}