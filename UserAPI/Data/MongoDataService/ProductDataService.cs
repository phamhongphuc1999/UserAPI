using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserAPI.Models.MongoModel;

namespace UserAPI.Data.MongoDataService
{
    public class ProductDataService: BaseDataService<Product>
    {
        public ProductDataService(string collection): base(collection)
        {

        }

        public bool InsertOneProduct(InsertProduct entity, string userId)
        {
            Product checkProduct = mCollection.Find(x => x.name == entity.name).FirstOrDefault();
            if (checkProduct != null) return false;
            Product product = new Product
            {
                name = entity.name,
                userId = userId,
                price = entity.price,
                createAt = DateTime.Now
            };
            mCollection.InsertOne(product);
            return true;
        }

        public async Task<bool> InsertOneProductAsync(InsertProduct entity, string userId)
        {
            Product checkProduct = await mCollection.Find(x => x.name == entity.name).FirstOrDefaultAsync();
            if (checkProduct != null) return false;
            Product product = new Product
            {
                name = entity.name,
                userId = userId,
                price = entity.price,
                createAt = DateTime.Now
            };
            await mCollection.InsertOneAsync(product);
            return true;
        }

        //public bool InsertManyProducts(List<InsertProduct> entities)
        //{

        //}

        //public async Task<bool> InsertManyProductsAsync(List<InsertProduct> entities)
        //{

        //}
    }
}
