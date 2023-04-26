using MongoDB.Driver;
using static UserAPI.Program;

namespace UserAPI.Services.MongoService.MongoDataService
{
  public class BaseDataService<T>
  {
    protected IMongoCollection<T> mCollection;

    public BaseDataService(string collection)
    {
      mCollection = APIConnector.Mongo.MDatabase.GetCollection<T>(collection);
    }
  }
}
