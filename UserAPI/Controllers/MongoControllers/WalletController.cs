// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;
using UserAPI.Services.JWTService;
using UserAPI.Services.MongoService;

namespace UserAPI.Controllers.MongoControllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private WalletService walletService;
        private IOptions<JWTConfig> _jwtConfig;
        private ILogger<WalletController> logger;
        private IAuthService authService;

        public WalletController(IOptions<JWTConfig> jwtConfig, ILogger<WalletController> logger)
        {
            this.logger = logger;
            walletService = new WalletService("MoneyLover", "Wallet");
            _jwtConfig = jwtConfig;
            authService = new JWTService(_jwtConfig.Value.SecretKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newWallet"></param>
        /// <returns></returns>
        [HttpPost("/wallets")]
        [CustomAuthorization]
        public async Task<object> CreateNewWallet([FromBody] NewWalletInfo newWallet)
        {
            try
            {
                string token = HttpContext.Request.Headers["token"];
                List<Claim> claims = authService.GetTokenClaims(token).ToList();
                string username = claims.Find(x => x.Type == ClaimTypes.Name).Value;
                Result result = await walletService.InsertWalletAsync(username, newWallet);
                if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
                return Ok(Responder.Success("success"));
            }
            catch(Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpGet("/wallets/{walletId}")]
        [CustomAuthorization]
        public async Task<object> GetWalletById(string walletId)
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("/wallets/all")]
        [CustomAuthorization]
        public async Task<object> GetWalletByUser()
        {
            try
            {
                string token = HttpContext.Request.Headers["token"];
                List<Claim> claims = authService.GetTokenClaims(token).ToList();
                string username = claims.Find(x => x.Type == ClaimTypes.Name).Value;
                logger.LogWarning(username);
                Result result = await walletService.GetWalletsByUserAsync(username);
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
