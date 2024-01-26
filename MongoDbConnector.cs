using System;
using System.Collections.Generic;
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
            Console.WriteLine("Erro: O banco de dados não foi inicializado corretamente.");
            return;
        }

        var collection = _database.GetCollection<Dictionary<string, object>>("webcrawler_logs");
        log["_id"] = ObjectId.GenerateNewId(); // Adiciona um identificador único ao log

        collection.InsertOne(log);
    }
}