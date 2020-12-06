// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using UserAPI.Models.CommonModel;
using UserAPI.Models.ElasticsearchModel;
using UserAPI.Services.ElasticsearchService;

namespace UserAPI.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class ElasticsearchTestController : ControllerBase
    {
        private BaseService elasService;

        public ElasticsearchTestController(IConfiguration configuration)
        {
            Uri[] uris = configuration.GetValue<Uri[]>("ElasticsearchSetting:Uris");
            elasService = new BaseService(uris);
        }

        /// <summary>Save data use database elasticsearch</summary>
        /// <param name="data">data that you want to save</param>
        /// <response code="200">return notice</response>
        /// <response code="400">if get mistake</response>
        /// <returns></returns>
        [HttpPost]
        [Route("/elasticsearch/save-data")]
        public object SaveData([FromBody] SaveData<string> data)
        {
            try
            {
                Models.CommonModel.Result result = elasService.SaveDocument<string>(data.data, data.id);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch(Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>query data from elasticsearch</summary>
        /// <param name="id">the id of data you want to query</param>
        /// <response code="200">return notice</response>
        /// <response code="400">if get mistake</response>
        /// <returns></returns>
        [HttpGet]
        [Route("/elasticsearch/query-data")]
        public object QueryData([FromQuery] Id id)
        {
            try
            {
                Models.CommonModel.Result result = elasService.QueryDocument<string>(id);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }
    }
}
