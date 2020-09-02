using MongoDB.Driver;

namespace MongoDatabase.Models
{
    public class BaseModel<T>
    {
        protected MongoClient client;
        protected IMongoDatabase mDatabase;
        protected IMongoCollection<T> mCollection;

        public BaseModel()
        {
            client = new MongoClient(Config.MONGO_SCRIPT);
        }
    }
}
