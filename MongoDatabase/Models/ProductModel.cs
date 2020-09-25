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
            mCollection = mDatabase.GetCollection<Product>("product_list");
        }

        public Result InsertProduct(NewProductInfo entity)
        {
            Product product = mCollection.Find(x => x.name == entity.name).ToList().FirstOrDefault();
            if (product != null) return new Result
            {
                status = 400,
                data = $"product name {entity.name} have existed"
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
            mCollection.InsertOne(newProduct);
            return new Result
            {
                status = 200,
                data = newProduct
            };
        }

        public async Task<Result> InsertProductAsync(NewProductInfo entity)
        {
            Product product = mCollection.Find(x => x.name == entity.name).ToList().FirstOrDefault();
            if (product != null) return new Result
            {
                status = 400,
                data = $"product name {entity.name} have existed"
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

        public Result GetProductById(string productId, string[] fields = null)
        {
            List<Product> result = mCollection.Find(x => x._id == productId).ToList();
            Product product = result.FirstOrDefault();
            if (product == null) return new Result
            {
                status = 400,
                data = $"the product with id: {productId} do not exist"
            };
            if (fields == null) return new Result
            {
                status = 200,
                data = product
            };
            Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (string field in fields)
                if (Config.productFields.ContainsKey(field))
                    data.Add(field, product.GetType().GetProperty(field).GetValue(product));
            return new Result
            {
                status = 200,
                data = data
            };
        }

        public async Task<Result> GetProductByIdAsync(string productId, string[] fields = null)
        {
            List<Product> result = await mCollection.Find(x => x._id == productId).ToListAsync();
            Product product = result.FirstOrDefault();
            if (product == null) return new Result
            {
                status = 400,
                data = $"the product with id: {productId} do not exist"
            };
            if (fields == null) return new Result
            {
                status = 200,
                data = product
            };
            Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (string field in fields)
                if (Config.productFields.ContainsKey(field))
                    data.Add(field, product.GetType().GetProperty(field).GetValue(product));
            return new Result
            {
                status = 200,
                data = data
            };
        }

        public Result GetListProduct(int pageSize = 0, int pageIndex = 0, string[] fields = null)
        {
            List<Product> productList = mCollection.Find(x => x.name != String.Empty).ToList();
            int totalResult = productList.Count;
            if (pageSize == 0) pageSize = totalResult;
            if (pageIndex == 0) pageIndex = 1;
            int index = pageSize * (pageIndex - 1);
            if(fields == null) return new Result
            {
                status = 200,
                data = new
                {
                    product_list = productList.GetRange(index, pageSize),
                    pagination = new
                    {
                        totalResult = totalResult,
                        pageIndex = pageIndex,
                        pageSize = pageSize
                    }
                }
            };
            List<Product> tempList = productList.GetRange(index, pageSize);
            IEnumerable<Dictionary<string, object>> productFilterList = tempList.Select(e =>
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (string field in fields)
                {
                    object value = e.GetType().GetProperty(field).GetValue(e);
                    result.Add(field, value);
                }
                return result;
            });
            return new Result
            {
                status = 200,
                data = new
                {
                    user_list = productFilterList,
                    pagination = new
                    {
                        totalResult = totalResult,
                        pageIndex = pageIndex,
                        pageSize = pageSize
                    }
                }
            };
        }

        public async Task<Result> GetListProductAsync(int pageSize = 0, int pageIndex = 0, string[] fields = null)
        {
            List<Product> productList = await mCollection.Find(x => x.name != String.Empty).ToListAsync();
            int totalResult = productList.Count;
            if (pageSize == 0) pageSize = totalResult;
            if (pageIndex == 0) pageIndex = 1;
            int index = pageSize * (pageIndex - 1);
            if (fields == null) return new Result
            {
                status = 200,
                data = new
                {
                    product_list = productList.GetRange(index, pageSize),
                    pagination = new
                    {
                        totalResult = totalResult,
                        pageIndex = pageIndex,
                        pageSize = pageSize
                    }
                }
            };
            List<Product> tempList = productList.GetRange(index, pageSize);
            IEnumerable<Dictionary<string, object>> productFilterList = tempList.Select(e =>
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (string field in fields)
                {
                    object value = e.GetType().GetProperty(field).GetValue(e);
                    result.Add(field, value);
                }
                return result;
            });
            return new Result
            {
                status = 200,
                data = new
                {
                    user_list = productFilterList,
                    pagination = new
                    {
                        totalResult = totalResult,
                        pageIndex = pageIndex,
                        pageSize = pageSize
                    }
                }
            };
        }

        public Result UpdateProduct(string productId, UpdateProductInfo updateProduct)
        {
            UpdateDefinition<Product> updateBuilder = Builders<Product>.Update.Set(x => x.updateAt, Hepler.CurrentTime());
            if (updateProduct.name != null)
            {
                Product checkProduct = mCollection.Find(x => x.name == updateProduct.name).FirstOrDefault();
                if (checkProduct != null) return new Result
                {
                    status = 400,
                    data = $"the product name: {updateProduct.name} is exist"
                };
                updateBuilder = updateBuilder.Set(x => x.name, updateProduct.name);
            }
            if (updateProduct.origin != null) updateBuilder = updateBuilder.Set(x => x.origin, updateProduct.origin);
            if (updateProduct.amount > 0) updateBuilder = updateBuilder.Set(x => x.amount, updateProduct.amount);
            if (updateProduct.price > 0) updateBuilder = updateBuilder.Set(x => x.price, updateProduct.price);
            if (updateProduct.guarantee > 0) updateBuilder = updateBuilder.Set(x => x.guarantee, updateProduct.guarantee);
            if (updateProduct.sale > 0) updateBuilder = updateBuilder.Set(x => x.sale, updateProduct.sale);
            if (updateProduct.status != null) updateBuilder = updateBuilder.Set(x => x.status, updateProduct.status);
            Product product = mCollection.FindOneAndUpdate(x => x._id == productId, updateBuilder);
            if (product != null) return new Result
            {
                status = 200,
                data = product
            };
            else return new Result
            {
                status = 400,
                data = $"do not update product with id: {productId}"
            };
        }

        public async Task<Result> UpdateProductAsync(string productId, UpdateProductInfo updateProduct)
        {
            UpdateDefinition<Product> updateBuilder = Builders<Product>.Update.Set(x => x.updateAt, Hepler.CurrentTime());
            if (updateProduct.name != null)
            {
                Product checkProduct = mCollection.Find(x => x.name == updateProduct.name).FirstOrDefault();
                if(checkProduct != null) return new Result
                {
                    status = 400,
                    data = $"the product name: {updateProduct.name} is exist"
                };
                updateBuilder = updateBuilder.Set(x => x.name, updateProduct.name);
            }
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
                data = $"do not update product with id: {productId}"
            };
        }

        public Result DeleteProduct(string productId)
        {
            Product product = mCollection.FindOneAndDelete(x => x._id == productId);
            if (product != null) return new Result
            {
                status = 200,
                data = product
            };
            else return new Result
            {
                status = 400,
                data = $"do not delete product with id: {productId}"
            };
        }

        public async Task<Result> DeleteProductAsync(string productId)
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
                data = $"do not delete product with id: {productId}"
            };
        }
    }
}
