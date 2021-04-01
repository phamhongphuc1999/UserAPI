// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;

namespace UserAPI.Controllers.MongoControllers
{
    public class ProductController: BaseMongoController
    {
        public ProductController(IOptions<JWTConfig> jwtConfig): base(jwtConfig)
        {
        }

        //[CustomAuthorization]
        //public async Task<object> InsertOneProduct([FromBody] InsertProduct entity)
        //{
        //    try
        //    {

        //    }
        //    catch(Exception error)
        //    {
        //        return BadRequest(Responder.Fail(error.Message));
        //    }
        //}
    }
}
