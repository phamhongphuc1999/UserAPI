using MongoDB.Driver;
using static UserAPI.Program;

namespace UserAPI.Services.MongoService
{
    public class BaseService<T>
    {
        protected IMongoCollection<T> mCollection;

        public BaseService(string collection)
        {
            mCollection = mongoConnecter.MDatabase.GetCollection<T>(collection);
        }
    }
}
