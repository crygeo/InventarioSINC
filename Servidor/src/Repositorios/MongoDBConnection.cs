using MongoDB.Driver;

namespace Servidor.src.Repositorios
{
    public class MongoDBConnection
    {
        public static IMongoDatabase? _database;

        public MongoDBConnection()
        {
            if (_database == null)
            {
                string connectionString = "mongodb://localhost:27017"; // Cambia esto a tu cadena de conexión
                string databaseName = "inventasinc";

                var client = new MongoClient(connectionString);
                _database = client.GetDatabase(databaseName);
            }
        }
    }
}
