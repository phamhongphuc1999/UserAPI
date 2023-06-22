using System.Threading.Tasks;
using UserAPI.Configuration;
using UserAPI.Services.MongoService.MongoDataService;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;
using MongoDB.Driver;
using MongoDB.Bson;

namespace UserAPI.Services.MongoService
{
  public class ProductService
  {
    private ProductDataService service;

    public ProductService(string collection)
    {
      service = new ProductDataService(collection);
    }

    public async Task<Result> AddNewProduct(InsertProduct insertedProduct, string userId)
    {
      bool result = await service.InsertOneProduct(insertedProduct, userId);
      if (result) return new Result
      {
        status = Status.Created,
        data = Messages.OK
      };
      return new Result
      {
        status = Status.BadRequest,
        data = Messages.BadRequest
      };
    }

    public async Task<Result> GetProductById(string productId, string[] fields = null)
    {
      FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
      FilterDefinition<BsonDocument> filter = builder.Eq("_id", ObjectId.Parse(productId));
      Product product = await service.GetSingleProduct(filter, fields);
      if (product == null) return new Result
      {
        status = Status.BadRequest,
        data = Messages.NotExistedUser
      };
      return new Result
      {
        status = Status.OK,
        data = product
      };
    }
  }
}
