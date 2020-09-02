using MongoDB.Driver;

namespace MongoDB.Models
{
    public class BaseModel<T>
    {
        protected MongoClient client;
        protected IMongoDatabase mCollection;
        protected IMongoCollection<T> mDocument;

        public BaseModel()
        {
            client = new MongoClient(Config.MONGO_SCRIPT);
        }
    }
}
