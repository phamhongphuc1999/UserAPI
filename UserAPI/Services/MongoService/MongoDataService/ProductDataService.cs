using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserAPI.Models.MongoModel;

namespace UserAPI.Services.MongoService.MongoDataService
{
  public class ProductDataService : BaseDataService<BsonDocument>
  {
    public ProductDataService(string collection) : base(collection) { }

    public async Task<bool> InsertOneProduct(InsertProduct entity, string userId)
    {
      try
      {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("name", entity.name);
        BsonDocument product = await mCollection.Find(filter).FirstOrDefaultAsync();
        if (product != null) return false;
        BsonDocument newProduct = new BsonDocument
                {
                    { "name", entity.name},
                    { "price", entity.price },
                    {"userId", userId },
                    { "createAt", DateTime.Now },
                    { "updateAt", DateTime.Now }
                };
        await mCollection.InsertOneAsync(newProduct);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public async Task<Product> GetSingleProduct(FilterDefinition<BsonDocument> filter, string[] fields = null)
    {
      BsonDocument product;
      if (fields == null) product = await mCollection.Find(filter).FirstOrDefaultAsync();
      else
      {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        foreach (string field in fields) dic.Add(field, 1);
        ProjectionDefinition<BsonDocument> projection = new BsonDocument(dic);
        product = await mCollection.Find(filter).Project(projection).FirstOrDefaultAsync();
      }
      return BsonSerializer.Deserialize<Product>(product);
    }
  }
}
