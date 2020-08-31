using Model;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using MongoDB.Bson;
using Model.Entities;
using System.Collections.Generic;
using UserAPI.Models;
using UserAPI.JWT;

namespace UserAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IOptions<DevelopmentConfig> _developmentConfig;
        private readonly IOptions<JWTConfig> _jwtConfig;
        private mongodb data;

        public UserController(ILogger<UserController> logger, IOptions<DevelopmentConfig> developmentConfig, 
            IOptions<JWTConfig> jwtConfig)
        {
            _logger = logger;
            _developmentConfig = developmentConfig;
            _jwtConfig = jwtConfig;
            data = new mongodb();
        }

        /// <summary>
        /// login account
        /// </summary>
        /// <returns></returns>
        [HttpGet("/login")]
        public async Task<object> Login()
        {
            try
            {
                string token = Request.Headers["token"];
                if (token != "null") return Ok(Responder.Fail("Already logined"));
                StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
                string userInfo = await reader.ReadToEndAsync();
                Dictionary<string, string> info = JsonConvert.DeserializeObject<Dictionary<string, string>>(userInfo);
                Result result = await data.Login(info["username"], info["password"]);
                if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
                node1:
                IAuthContainerModel model = Helper.GetJWTContainerModel(info["username"], info["password"], _jwtConfig);
                IAuthService authService = new JWTService(model.SecretKey);
                string accessToken = authService.GenerateToken(model);
                if (!authService.IsTokenValid(accessToken)) goto node1;
                return Ok(Responder.Success(new { access_token = accessToken }));
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

        /// <summary>
        /// logout account
        /// </summary>
        /// <returns></returns>
        [HttpGet("/logout")]
        public async Task<object> Logout()
        {
            try
            {
                string token = Request.Headers["token"];
                if (token == "null") return Ok(Responder.Fail("Already logouted"));
                return Ok(Responder.Success(new { access_token = "null" }));
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

        /// <summary>
        /// create new user
        /// </summary>
        /// <returns></returns>
        [HttpPost("/users")]
        public async Task<object> CreateNewUser()
        {
            try
            {
                _logger.LogInformation("POST: {0}/users", _developmentConfig.Value.ApplicationUrl);
                StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
                string userInfo = await reader.ReadToEndAsync();
                User newUser = JsonConvert.DeserializeObject<User>(userInfo);
                Result result = await data.InsertUser(newUser);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
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

        /// <summary>
        /// get user by id
        /// </summary>
        /// <param name="userId">the user id who want to get</param>
        /// <returns></returns>
        [HttpGet("/users/{userId}")]
        public async Task<object> GetUserById(string userId)
        {
            try
            {
                _logger.LogInformation("GET: {0}/users/{1}", _developmentConfig.Value.ApplicationUrl, userId);
                string fieldsString = Request.Query["fields"];
                Result result;
                if (fieldsString != null)
                {
                    string[] fields = fieldsString.Split(',');
                    result = await data.GetUserById(userId, fields);
                }
                else result = await data.GetUserById(userId);
                if(result.status == 200) return Ok(Responder.Success(result.data));
                return StatusCode(result.status, Responder.Fail(result.data));
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

        /// <summary>
        /// get list user
        /// </summary>
        /// <returns></returns>
        [HttpGet("/users")]
        public async Task<object> GetListUser()
        {
            try
            {
                _logger.LogInformation("GET: {0}/users", _developmentConfig.Value.ApplicationUrl);
                string pageSize = Request.Query["page_size"];
                string pageIndex = Request.Query["page-index"];
                Result result = await data.GetListUser();
                return Ok(Responder.Success(result.data));
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

        /// <summary>
        /// update user
        /// </summary>
        /// <param name="userId">the user id who want to update</param>
        /// <returns></returns>
        [HttpPut("/users/{userId}")]
        public async Task<object> UpdateUser(string userId)
        {
            try
            {
                _logger.LogInformation("PUT: {0}/users/{1}", _developmentConfig.Value.ApplicationUrl, userId);
                StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
                string userInfo = await reader.ReadToEndAsync();
                User updateUser = JsonConvert.DeserializeObject<User>(userInfo);
                Result result = await data.UpdateUser(userId, updateUser);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
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

        /// <summary>
        /// update role of a user
        /// </summary>
        /// <param name="userId">the user id who want to update role</param>
        /// <returns></returns>
        [HttpPut("/admin/users/{userId}")]
        public async Task<object> UpdateRole(string userId)
        {
            try
            {
                StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
                string userInfo = await reader.ReadToEndAsync();
                User user = JsonConvert.DeserializeObject<User>(userInfo);
                Result result = await data.UpdateRole(userId, user);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
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

        /// <summary>
        /// delete a user
        /// </summary>
        /// <param name="userId">the user id who want to delete</param>
        /// <returns></returns>
        [HttpDelete("/users/{userId}")]
        public async Task<object> DeleteUser(string userId)
        {
            try
            {
                _logger.LogInformation("DELETE: {0}/users/{1}", _developmentConfig.Value.ApplicationUrl, userId);
                Result result = await data.DeleteUser(userId);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
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
