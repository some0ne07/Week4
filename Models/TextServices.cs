using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApp3.Models
{
    public class TextServices
    {
        private readonly IMongoCollection<BsonDocument> collection;
        public TextServices()
        {
            var dbclient = new MongoClient("mongodb://localhost:27017");
            var database = dbclient.GetDatabase("txtDb");
            collection = database.GetCollection<BsonDocument>("txts");
        }

        public async Task Addocs(BsonDocument doc)
        {
            await collection.InsertOneAsync(doc);
        }
        public async Task<List<BsonDocument>> Get()
        {
            return await collection.Find(new BsonDocument()).Sort("{_id:-1}").ToListAsync();
        }

        public async Task UpdateDocs(string imgId, string newTxt)
        {
            BsonDocument filter = new BsonDocument()
            { { "imgId", imgId } };
            BsonDocument doc = await collection.Find(filter).SingleAsync();

            doc["txt"] = newTxt;

            await collection.FindOneAndUpdateAsync(filter,doc);
        }

        [Obsolete]
        public async Task UpdateImgId(string imId, string newImgId)
        {
            BsonDocument filter = new BsonDocument()
            { { "imgId", imId } };
            BsonDocument doc = await collection.Find(filter).SingleAsync();

            doc["imgId"] = newImgId;
            await collection.FindOneAndUpdateAsync(filter,doc);
        }
    }
}
