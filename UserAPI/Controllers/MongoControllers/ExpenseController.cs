﻿// Copyright (c) Microsoft. All Rights Reserved.
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
    public class ExpenseController : ControllerBase
    {
        private ExpenseService expenseService;
        public ExpenseController()
        {
            expenseService = new ExpenseService("MoneyLover", "Expense");
        }

        [HttpPost("/expenses/{categoryId}")]
        public async Task<object> CreateNewExpense(string cateforyId, [FromBody] NewExpenseInfo newExpense)
        {
            try
            {
                Result result = await expenseService.InsertExpenseAsync(cateforyId, newExpense);
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