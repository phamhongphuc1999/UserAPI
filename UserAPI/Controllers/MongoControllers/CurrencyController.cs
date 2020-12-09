// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;
using UserAPI.Services.MongoService;

namespace UserAPI.Controllers.MongoControllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private CurrencyService currencyService;

        public CurrencyController(IOptions<MongoSetting> options)
        {
            currencyService = new CurrencyService(options, "Currency");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newCurrency"></param>
        /// <returns></returns>
        [HttpPost("/currencies")]
        public async Task<object> CreateNewCurrency([FromBody] NewCurrencyInfo newCurrency)
        {
            try
            {
                Result result = await currencyService.InsertCurrencyAsync(newCurrency.iconId, newCurrency.name);
                if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
                return Ok(Responder.Success(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }
    }
}
