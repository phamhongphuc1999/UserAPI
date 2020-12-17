// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;
using static UserAPI.Program;

namespace UserAPI.Controllers.MongoControllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        [HttpPost("/budgets/{walletId}")]
        [CustomAuthorization]
        public async Task<object> CreateNewBudget(string walletId, NewBudgetInfo newBudget)
        {
            try
            {
                Result result = await budgetService.InsertBudgetAsync(walletId, newBudget);
                if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
                return Ok(Responder.Success("success"));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

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
