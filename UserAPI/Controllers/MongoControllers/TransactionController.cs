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
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private TransactionService transactionService;

        public TransactionController()
        {
            transactionService = new TransactionService("MoneyLover", "Transaction");
        }

        /// <summary>Create new transaction</summary>
        /// <remarks>create new transaction</remarks>
        /// <param name="newTransaction">The instance representing new transaction</param>
        /// <param name="walletId"></param>
        /// <returns></returns>
        [HttpPost("/transactions/{walletId}")]
        [CustomAuthorization]
        public async Task<object> CreateNewTransaction(string walletId, [FromBody] NewTransactionInfo newTransaction)
        {
            try
            {
                bool check = (newTransaction.amount <= 0) && (newTransaction.date == null);
                if (!check) return StatusCode(401, Responder.Fail("Fill complete"));
                Result result = await transactionService.InsertTransactionAsync(walletId, newTransaction);
                return StatusCode(result.status, Responder.Success("success"));
            }
            catch(Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        [HttpGet("/transactions/{transactionId}")]
        [CustomAuthorization]
        public async Task<object> GetTransactionById(string transactionId)
        {
            try
            {
                Result result = await transactionService.GetTransactionByIdAsync(transactionId);
                if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
                return Ok(Responder.Success(result.data));
            }
            catch(Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        [HttpGet("/transactions/{walletId}")]
        [CustomAuthorization]
        public async Task<object> GetTransactionsByWallet(string walletId)
        {
            try
            {
                Result result = await transactionService.GetTransactionsByWalletAsync(walletId);
                return Ok(Responder.Success(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }
    }
}
