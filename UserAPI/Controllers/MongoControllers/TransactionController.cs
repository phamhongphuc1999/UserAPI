﻿// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;
using UserAPI.Services.JWTService;
using static UserAPI.Program;

namespace UserAPI.Controllers.MongoControllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IOptions<JWTConfig> _jwtConfig;
        private IAuthService authService;

        public TransactionController(IOptions<JWTConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig;
            authService = new JWTService(_jwtConfig.Value.SecretKey);
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
                string token = HttpContext.Request.Headers["token"];
                List<Claim> claims = authService.GetTokenClaims(token).ToList();
                string username = claims.Find(x => x.Type == ClaimTypes.Name).Value;
                Result result = await transactionService.InsertTransactionAsync(walletId, username, newTransaction);
                return StatusCode(result.status, Responder.Success(result.data));
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
        /// <param name="fields"></param>
        /// <param name="transactionInfo"></param>
        /// <returns></returns>
        [HttpGet("/transactions")]
        [CustomAuthorization]
        public async Task<object> GetTransactionsByWallet([FromQuery] string fields, [FromBody] GetTransactionInfo transactionInfo)
        {
            try
            {
                string token = HttpContext.Request.Headers["token"];
                List<Claim> claims = authService.GetTokenClaims(token).ToList();
                string username = claims.Find(x => x.Type == ClaimTypes.Name).Value;
                Result result;
                if(fields != null)
                {
                    string[] fArray = Helper.SplipFields(fields);
                    result = await transactionService.GetListTransactionsAsync(transactionInfo, username, fArray);
                }
                else
                {
                    string[] fArray = Helper.SplipFields("amount,createAt");
                    result = await transactionService.GetListTransactionsAsync(transactionInfo, username, fArray);
                }
                return Ok(Responder.Success(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="updateTransaction"></param>
        /// <returns></returns>
        [HttpPut("/transactions/{transactionId}")]
        [CustomAuthorization]
        public async Task<object> UpdateTransaction(string transactionId, [FromBody] UpdateTransactionInfo updateTransaction)
        {
            try
            {
                string token = HttpContext.Request.Headers["token"];
                List<Claim> claims = authService.GetTokenClaims(token).ToList();
                string username = claims.Find(x => x.Type == ClaimTypes.Name).Value;
                Result result = await transactionService.UpdateTransactionAsync(updateTransaction, transactionId, username);
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
