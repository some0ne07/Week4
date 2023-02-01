using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.IO;

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

        public async Task<BsonObjectId> AddDocs(string fileName, byte[] source)
        {
            return await bucket.UploadFromBytesAsync(fileName,source);
        }
        public async Task<byte[]> Get(string imgId)
        {
            return await bucket.DownloadAsBytesAsync(ObjectId.Parse(imgId));
        }
        public async Task DeleteDocs(string imgId)
        {
            await bucket.DeleteAsync(ObjectId.Parse(imgId));
        }

    }
}
