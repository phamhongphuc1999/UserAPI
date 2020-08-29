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
        [Route("/login")]
        public async Task<object> Login()
        {
            try
            {
                return "success";
            }
            catch (BsonException error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
            catch (Exception error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpPost]
        [Route("/logout")]
        public async Task<object> Logout()
        {
            try
            {
                return "success";
            }
            catch (BsonException error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
            catch (Exception error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
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
                if (result) return Ok(Responder.Success());
                else return BadRequest(Responder.Fail("Can not create new user"));
            }
            catch (BsonException error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
            catch (Exception error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpGet]
        [Route("/users/{userId}")]
        public async Task<object> GetUserById(string userId)
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
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
            catch (Exception error)
            {
                _logger.LogError("{0}", error.Message);
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
                return Responder.Success(userList);
            }
            catch (BsonException error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
            catch (Exception error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpPut]
        [Route("/users/{userId}")]
        public async Task<object> UpdateUser(string userId)
        {
            try
            {
                _logger.LogInformation("PUT: {0}/users/{1}", _config.Value.ApplicationUrl, userId);
                StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
                string userInfo = await reader.ReadToEndAsync();
                User updateUser = JsonConvert.DeserializeObject<User>(userInfo);
                bool result = await data.UpdateUser(userId, updateUser);
                if (result) return Ok(Responder.Success());
                else return BadRequest(Responder.Fail($"Fail to update user with id: {userId}"));
            }
            catch (BsonException error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
            catch (Exception error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpPut]
        [Route("/admin/users/{userId}")]
        public async Task<object> UpdateRole(string userId)
        {
            try
            {
                return Responder.Success("success");
            }
            catch (BsonException error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
            catch (Exception error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpDelete]
        [Route("/users/{userId}")]
        public async Task<object> DeleteUser(string userId)
        {
            try
            {
                _logger.LogInformation("DELETE: {0}/users/{1}", _config.Value.ApplicationUrl, userId);
                bool result = await data.DeleteUser(userId);
                if (result) return Ok(Responder.Success());
                else return BadRequest(Responder.Fail($"Fail to delete user with id: {userId}"));
            }
            catch (BsonException error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
            catch (Exception error)
            {
                _logger.LogError("{0}", error.Message);
                return BadRequest(Responder.Fail(error.Message));
            }
        }
    }
}
