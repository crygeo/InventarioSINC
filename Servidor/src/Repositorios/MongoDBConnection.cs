using MongoDB.Driver;

namespace Servidor.Repositorios;

public class MongoDBConnection
{
    public static IMongoDatabase? _database;

    public MongoDBConnection()
    {
        if (_database == null)
        {
            var connectionString = "mongodb://localhost:27017"; // Cambia esto a tu cadena de conexión
            var databaseName = "inventasinc";

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }
    }
}