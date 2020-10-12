using MongoDB.Driver;

namespace UserAPI.Services.MongoService
{
    public class BaseService<T>
    {
        protected MongoClient client;
        protected IMongoDatabase mDatabase;
        protected IMongoCollection<T> mCollection;

        public BaseService()
        {
            client = new MongoClient(Config.MONGO_SCRIPT);
            mDatabase = client.GetDatabase("User");
        }
    }
}
