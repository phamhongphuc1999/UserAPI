// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;

namespace UserAPI.Services.MongoService
{
    public class ProductService: BaseService<Product>
    {
        public ProductService(string collection) : base(collection) { }

        public Result InsertProduct(InsertProduct insertProduct, string userId)
        {

        }
    }
}
