// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Models.CommonModel;
using UserAPI.Models.MongoModel;
using UserAPI.Services.MongoService;

namespace UserAPI.Controllers.MongoControllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private BudgetService budgetService;

        public BudgetController()
        {
            budgetService = new BudgetService("MoneyLover", "Budget");
        }

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
    }
}
