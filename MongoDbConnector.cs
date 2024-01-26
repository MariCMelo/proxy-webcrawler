using MongoDB.Bson;
using MongoDB.Driver;

public class MongoDbConnector
{
    private readonly IMongoDatabase? _database;

    public MongoDbConnector(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public void AddLog(Dictionary<string, object> log)
    {
        if (_database == null)
        {
            Console.WriteLine("Error: The database was not initialized correctly.");
            return;
        }

        var collection = _database.GetCollection<Dictionary<string, object>>("webcrawler_logs");
        // Adds a unique identifier to the log.
        log["_id"] = ObjectId.GenerateNewId();

        collection.InsertOne(log);
    }
}