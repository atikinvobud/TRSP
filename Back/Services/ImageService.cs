using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.IO;
using System.Threading.Tasks;
using Back.Models;

namespace Back.Services;

public class ImageService
{
    private readonly GridFSBucket gridFsBucket;

    public ImageService(MongoDbContext context)
    {
        gridFsBucket = context.GridFsBucket;
    }

    public async Task<ObjectId> UploadAsync(string fileName, string contentType, Stream stream)
    {
        var options = new GridFSUploadOptions
        {
            Metadata = new BsonDocument
            {
                { "contentType", contentType }
            }
        };

        return await gridFsBucket.UploadFromStreamAsync(fileName, stream, options);
    }

    public async Task<GridFSDownloadStream> DownloadAsync(string id)
    {
        var objectId = ObjectId.Parse(id);
        return await gridFsBucket.OpenDownloadStreamAsync(objectId);
    }

    public async Task<GridFSFileInfo> GetFileInfoAsync(string id)
    {
        var objectId = ObjectId.Parse(id);
        var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Id, objectId);
        var cursor = await gridFsBucket.FindAsync(filter);
        return await cursor.FirstOrDefaultAsync();
    }
}