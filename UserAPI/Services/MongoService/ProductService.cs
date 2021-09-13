// -------------------- SIMPLE API -------------------- 
//
//
// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
//
//
// Product by: Pham Hong Phuc
//
//
// ----------------------------------------------------

using System.Threading.Tasks;
using UserAPI.Contances;
using UserAPI.Services.MongoService.MongoDataService;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;

namespace UserAPI.Services.MongoService
{
    public class ProductService
    {
        private ProductDataService service;

        public ProductService(string collection)
        {
            service = new ProductDataService(collection);
        }

        public Result InsertOneProduct(InsertProduct insertProduct, string userId)
        {
            bool result = service.InsertOneProduct(insertProduct, userId);
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

        public async Task<Result> InsertOneProductAsync(InsertProduct insertProduct, string userId)
        {
            bool result = await service.InsertOneProductAsync(insertProduct, userId);
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
    }
}
