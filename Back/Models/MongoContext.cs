using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Back.Models;

public class MongoDbContext
{
    private readonly IMongoDatabase database;
    private readonly GridFSBucket gridFSBucket;

    public MongoDbContext(IMongoClient client, IConfiguration configuration)
    {
        database = client.GetDatabase(configuration["MongoSettings:Database"]);
        gridFSBucket = new GridFSBucket(database);
    }

    public IMongoDatabase Database => database;
    public GridFSBucket GridFsBucket => gridFSBucket;
}
