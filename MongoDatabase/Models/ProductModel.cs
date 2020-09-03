using MongoDB.Bson;
using MongoDB.Driver;
using MongoDatabase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDatabase.Models
{
    public class ProductModel : BaseModel<Product>
    {
        public ProductModel() : base()
        {
            mDatabase = client.GetDatabase("Product");
            mCollection = mDatabase.GetCollection<Product>("product_list");
        }

        public async Task<Result> InsertProduct(NewProductInfo entity)
        {
            Product product = mCollection.Find(x => x.name == entity.name).ToList().FirstOrDefault();
            if (product != null) return new Result
            {
                status = 400,
                data = $"username {entity.name} have existed"
            };
            Product newProduct = new Product()
            {
                name = entity.name,
                origin = entity.origin,
                amount = entity.amount,
                price = entity.price,
                guarantee = (entity.guarantee > 0) ? entity.guarantee : 0,
                sale = (entity.sale > 0) ? entity.sale : 0,
                createAt = Hepler.CurrentTime(),
                updateAt = Hepler.CurrentTime(),
                status = "enable"
            };
            await mCollection.InsertOneAsync(newProduct);
            return new Result
            {
                status = 200,
                data = newProduct
            };
        }

        public async Task<Result> GetProductById(string productId, string[] fields = null)
        {
            List<Product> result = await mCollection.Find(x => x._id == productId).ToListAsync();
            Product product = result.FirstOrDefault();
            if (product == null) return new Result
            {
                status = 400,
                data = $"the user with id: {productId} do not exist"
            };
            if (fields == null) return new Result
            {
                status = 200,
                data = product
            };
            BsonDocument sProduct = product.ToBsonDocument();
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (string field in fields)
                if (Config.productFields.ContainsKey(field))
                    data.Add(field, sProduct.GetElement(field).Value.ToString());
            return new Result
            {
                status = 200,
                data = data
            };
        }

        public async Task<Result> GetListProduct()
        {
            List<Product> productList = await mCollection.Find(x => x.name != String.Empty).ToListAsync();
            return new Result
            {
                status = 200,
                data = productList
            };
        }

        public async Task<Result> UpdateProduct(string productId, UpdateProductInfo updateProduct)
        {
            UpdateDefinition<Product> updateBuilder = Builders<Product>.Update.Set(x => x.updateAt, Hepler.CurrentTime());
            if (updateProduct.name != null) updateBuilder = updateBuilder.Set(x => x.name, updateProduct.name);
            if (updateProduct.origin != null) updateBuilder = updateBuilder.Set(x => x.origin, updateProduct.origin);
            if (updateProduct.amount > 0) updateBuilder = updateBuilder.Set(x => x.amount, updateProduct.amount);
            if (updateProduct.price > 0) updateBuilder = updateBuilder.Set(x => x.price, updateProduct.price);
            if (updateProduct.guarantee > 0) updateBuilder = updateBuilder.Set(x => x.guarantee, updateProduct.guarantee);
            if (updateProduct.sale > 0) updateBuilder = updateBuilder.Set(x => x.sale, updateProduct.sale);
            if (updateProduct.status != null) updateBuilder = updateBuilder.Set(x => x.status, updateProduct.status);
            Product product = await mCollection.FindOneAndUpdateAsync(x => x._id == productId, updateBuilder);
            if (product != null) return new Result
            {
                status = 200,
                data = product
            };
            else return new Result
            {
                status = 400,
                data = $"do not update user with id: {productId}"
            };
        }

        public async Task<Result> DeleteProduct(string productId)
        {
            Product product = await mCollection.FindOneAndDeleteAsync(x => x._id == productId);
            if (product != null) return new Result
            {
                status = 200,
                data = product
            };
            else return new Result
            {
                status = 400,
                data = $"do not delete user with id: {productId}"
            };
        }
    }
}
