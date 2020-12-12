// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using System;
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
    public class CategoryController : ControllerBase
    {
        [HttpPost("/categories")]
        public async Task<object> CreateNewCategory([FromBody] NewCategoryInfo newCategory)
        {
            try
            {
                Result result = await categoryService.InsertCategoryAsync(newCategory);
                if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
                return Ok(Responder.Success("success"));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpGet("/categories/{categoryId}")]
        public async Task<object> GetCategoryById(string categoryId)
        {
            try
            {
                Result result = await categoryService.GetCategoryByIdAsync(categoryId);
                if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
                return Ok(Responder.Success(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpGet("/categories")]
        public async Task<object> GetListCategories()
        {
            try
            {
                Result result = await categoryService.GetListCategoriesAsync();
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
