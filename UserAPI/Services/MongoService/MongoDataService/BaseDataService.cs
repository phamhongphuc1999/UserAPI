using MongoDB.Driver;
using UserAPI.Connector;

namespace UserAPI.Services.MongoService.MongoDataService
{
  public class BaseDataService<T>
  {
    protected IMongoCollection<T> mCollection;

    public BaseDataService(string collection)
    {
      mCollection = APIConnection.Mongo.MDatabase.GetCollection<T>(collection);
    }
  }
}
