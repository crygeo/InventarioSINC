using System.Configuration;

namespace Cliente.Services;

public static class Config
{
    public static string ServerUrl => ConfigurationManager.AppSettings["ServerUrl"] ?? string.Empty;
    public static int Port => int.Parse(ConfigurationManager.AppSettings["Port"] ?? "0");
    public static string ApiEndpoint => ConfigurationManager.AppSettings["ApiEndpoint"] ?? string.Empty;
    public static string FullUrl => $"{ServerUrl}:{Port}/{ApiEndpoint}";
    public static string Url => $"{ServerUrl}:{Port}";
}