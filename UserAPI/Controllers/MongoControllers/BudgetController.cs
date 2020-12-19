// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;
using UserAPI.Services.JWTService;
using static UserAPI.Program;

namespace UserAPI.Controllers.MongoControllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private IOptions<JWTConfig> _jwtConfig;
        private IAuthService authService;

        public BudgetController(IOptions<JWTConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig;
            authService = new JWTService(_jwtConfig.Value.SecretKey);
        }

        /// <summary>
        /// Create New budget
        /// </summary>
        /// <remarks>Create New budget</remarks>
        /// <param name="walletId">The wallet id that contain budget</param>
        /// <param name="newBudget">The new bidget information</param>
        /// <returns></returns>
        [HttpPost("/budgets/{walletId}")]
        [CustomAuthorization]
        public async Task<object> CreateNewBudget(string walletId, NewBudgetInfo newBudget)
        {
            try
            {
                string token = HttpContext.Request.Headers["token"];
                List<Claim> claims = authService.GetTokenClaims(token).ToList();
                string username = claims.Find(x => x.Type == ClaimTypes.Name).Value;
                Result result = await budgetService.InsertBudgetAsync(walletId, username, newBudget);
                if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
                return Ok(Responder.Success("success"));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="budgetId"></param>
        /// <returns></returns>
        [HttpGet("/budgets/{budgetId}")]
        [CustomAuthorization]
        public async Task<object> GetBudgetById(string budgetId)
        {
            try
            {
                Result result = await budgetService.GetBudgetByIdAsync(budgetId);
                if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
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
        /// <param name="walletId"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        [HttpGet("/budgets/{walletId}")]
        [CustomAuthorization]
        public async Task<object> GetBudgetsByWallet(string walletId, [FromQuery][Required] string categories)
        {
            try
            {
                Result result;
                if (categories != null)
                {
                    string[] categoriesList = Helper.SplipFields(categories);
                    result = await budgetService.GetBudgetsByWalletAsync(walletId, categoriesList);
                }
                else result = await budgetService.GetBudgetsByWalletAsync(walletId);
                return Ok(Responder.Success(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }
    }
}
