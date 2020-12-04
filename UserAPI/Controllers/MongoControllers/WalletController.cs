// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;
using UserAPI.Services.MongoService;

namespace UserAPI.Controllers.MongoControllers
{
    [ApiController]
    [Produces("application/json")]
    public class WalletController : ControllerBase
    {
        private WalletService walletService;

        public WalletController()
        {
            walletService = new WalletService("MoneyLover", "Wallet");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currencyId"></param>
        /// <param name="newWallet"></param>
        /// <returns></returns>
        [HttpPost("/wallets/{userId}")]
        public async Task<object> CreateNewWallet(string userId, [FromQuery] string currencyId, [FromBody] NewWalletInfo newWallet)
        {
            try
            {
                Result result = await walletService.InsertWalletAsync(userId, currencyId, newWallet);
                if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
                return Ok(Responder.Success("success"));
            }
            catch(Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpGet("/wallets/{walletId}")]
        public async Task<object> GetwalletById(string walletId)
        {
            try
            {
                Result result = await walletService.GetWalletByIdAsync(walletId);
                if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
                return Ok(Responder.Fail(result.data));
            }
            catch(Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }
    }
}
