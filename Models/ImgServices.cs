using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace WebApp3.Models
{
    public class ImgServices
    {
        private readonly IMongoCollection<BsonDocument> collection;
        private readonly IMongoDatabase database;
        private readonly GridFSBucket bucket;

        public ImgServices()
        {
            var dbclient = new MongoClient("mongodb://localhost:27017");
            database = dbclient.GetDatabase("imgDb");
            collection = database.GetCollection<BsonDocument>("imgs");
            bucket = new GridFSBucket(database);
        }

        public async Task Addocs(string filename, byte[] source)
        {
            await bucket.UploadFromBytesAsync(filename,source);
        }
        public async Task<byte[]> Get(string filename)
        {
            return await bucket.DownloadAsBytesByNameAsync(filename);
        }
    }
}
