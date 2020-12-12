// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserAPI.Models.CommonModel;
using static UserAPI.Program;

namespace UserAPI.Controllers.MongoControllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class IconController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpPost("/icons")]
        public async Task<object> CreateNewIcon([FromQuery] string url)
        {
            try
            {
                Result result = await iconService.InsertIconAsync(url);
                return result;
            }
            catch(Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpGet("/icon/{iconId}")]
        public async Task<object> GetIconById(string iconId)
        {
            try
            {
                Result result = await iconService.GetIconByIdAsync(iconId);
                if (result.status != 200) return StatusCode(result.status, result.data);
                return Ok(result.data);
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }
    }
}
