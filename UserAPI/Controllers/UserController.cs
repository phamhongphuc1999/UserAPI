using MongoDatabase.Models;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using MongoDatabase.Entities;
using System.Collections.Generic;
using UserAPI.Models;
using UserAPI.JWT;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace UserAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IOptions<JWTConfig> _jwtConfig;
        private UserModel userModel;

        public UserController(IOptions<JWTConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig;
            userModel = new UserModel();
        }
   
        [HttpGet("/login")]
        [Description("login with username and password")]
        [ProducesResponseType(200, Type = typeof(ResponseType))]
        [ProducesResponseType(400, Type = typeof(ResponseType))]
        public async Task<object> Login()
        {
            try
            {
                string token = Request.Headers["token"];
                if (token != "null") return Ok(Responder.Success("Already logined"));
                StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
                string userInfo = await reader.ReadToEndAsync();
                Dictionary<string, string> info = JsonConvert.DeserializeObject<Dictionary<string, string>>(userInfo);
                Result result = await userModel.Login(info["username"], info["password"]);
                if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
                node1:
                IAuthContainerModel model = Helper.GetJWTContainerModel(info["username"], info["password"], result.data.ToString(), _jwtConfig);
                IAuthService authService = new JWTService(model.SecretKey);
                string accessToken = authService.GenerateToken(model);
                if (!authService.IsTokenValid(accessToken)) goto node1;
                return Ok(Responder.Success(new { access_token = accessToken }));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpGet("/logout")]
        [ProducesResponseType(200, Type = typeof(ResponseType))]
        [ProducesResponseType(400, Type = typeof(ResponseType))]
        public async Task<object> Logout()
        {
            try
            {
                string token = Request.Headers["token"];
                if (token == "null") return Ok(Responder.Success("Already logouted"));
                return Ok(Responder.Success(new { access_token = "null" }));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpPost("/users")]
        [ProducesResponseType(200, Type = typeof(ResponseType))]
        [ProducesResponseType(400, Type = typeof(ResponseType))]
        public async Task<object> CreateNewUser()
        {
            try
            {
                StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
                string userInfo = await reader.ReadToEndAsync();
                User newUser = JsonConvert.DeserializeObject<User>(userInfo);
                Result result = await userModel.InsertUser(newUser);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpGet("/users/{userId}")]
        [ProducesResponseType(200, Type = typeof(ResponseType))]
        [ProducesResponseType(400, Type = typeof(ResponseType))]
        public async Task<object> GetUserById(string userId)
        {
            try
            {
                string fieldsString = Request.Query["fields"];
                Result result;
                if (fieldsString != null)
                {
                    string[] fields = fieldsString.Split(',');
                    result = await userModel.GetUserById(userId, fields);
                }
                else result = await userModel.GetUserById(userId);
                if(result.status == 200) return Ok(Responder.Success(result.data));
                return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpGet("/users")]
        [ProducesResponseType(200, Type = typeof(ResponseType))]
        [ProducesResponseType(400, Type = typeof(ResponseType))]
        public async Task<object> GetListUser()
        {
            try
            {
                string pageSize = Request.Query["page_size"];
                string pageIndex = Request.Query["page_index"];
                Result result = await userModel.GetListUser();
                return Ok(Responder.Success(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpPut("/users/{userId}")]
        [ProducesResponseType(200, Type = typeof(ResponseType))]
        [ProducesResponseType(400, Type = typeof(ResponseType))]
        public async Task<object> UpdateUser(string userId)
        {
            try
            {
                StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
                string userInfo = await reader.ReadToEndAsync();
                User updateUser = JsonConvert.DeserializeObject<User>(userInfo);
                Result result = await userModel.UpdateUser(userId, updateUser);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpPut("/admin/users/{userId}")]
        [ProducesResponseType(200, Type = typeof(ResponseType))]
        [ProducesResponseType(400, Type = typeof(ResponseType))]
        public async Task<object> UpdateRole(string userId)
        {
            try
            {
                StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
                string userInfo = await reader.ReadToEndAsync();
                User user = JsonConvert.DeserializeObject<User>(userInfo);
                string role = Request.Headers["role"];
                if (role == "user") return StatusCode(401, Responder.Fail("You not allow to update role"));
                Result result = await userModel.UpdateRole(userId, user);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpDelete("/users/{userId}")]
        [ProducesResponseType(200, Type = typeof(ResponseType))]
        [ProducesResponseType(400, Type = typeof(ResponseType))]
        public async Task<object> DeleteUser(string userId)
        {
            try
            {
                Result result = await userModel.DeleteUser(userId);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }
    }
}
