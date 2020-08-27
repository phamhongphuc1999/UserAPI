using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model;
using Newtonsoft.Json;
using MongoDB.Bson;

namespace UserAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IOptions<Config> _config;
        private mongodb data;

        public UserController(ILogger<UserController> logger, IOptions<Config> config)
        {
            _logger = logger;
            _config = config;
            data = new mongodb();
        }

        [HttpPost]
        [Route("/users")]
        public async Task<object> CreateNewUser()
        {
            try
            {
                _logger.LogInformation("POST: {0}/users", _config.Value.ApplicationUrl);
                StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
                string userInfo = await reader.ReadToEndAsync();
                User newUser = JsonConvert.DeserializeObject<User>(userInfo);
                bool result = await data.InsertUser(newUser);
                if (result) return Ok("success");
                else return BadRequest("fail");
            }
            catch (BsonException error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpGet]
        [Route("/users/{userId}")]
        public async Task<object> GetUSerById(string userId)
        {
            try
            {
                _logger.LogInformation("GET: {0}/users/{1}", _config.Value.ApplicationUrl, userId);
                //string filedsString = Request.Query["fileds"];
                //string[] fileds = filedsString.Split(',');

                User user = await data.GetUserById(userId);
                return Responder.Success(user);
            }
            catch (BsonException error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpGet]
        [Route("/users")]
        public async Task<object> GetListUser()
        {
            try
            {
                _logger.LogInformation("GET: {0}/users", _config.Value.ApplicationUrl);
                string pageSize = Request.Query["page_size"];
                string pageIndex = Request.Query["page-index"];
                List<User> userList = await data.GetListUser();
                return userList;
            }
            catch (BsonException error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpDelete]
        [Route("/users/{userId}")]
        public async Task<object> DeleteUSer(string userId)
        {
            try
            {
                _logger.LogInformation("DELETE: {0}/users/{1}", _config.Value.ApplicationUrl, userId);
                bool result = await data.DeleteUSer(userId);
                if (result) return "success";
                else return BadRequest($"Fail to delete user with id: {userId}");
            }
            catch (BsonException error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }
    }
}
