using MongoDatabase.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDatabase.Entities;
using UserAPI.Models;
using UserAPI.JWT;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace UserAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IOptions<JWTConfig> _jwtConfig;
        private readonly ILogger<UserController> _logger;
        private UserModel userModel;

        public UserController(IOptions<JWTConfig> jwtConfig, ILogger<UserController> logger)
        {
            _jwtConfig = jwtConfig;
            _logger = logger;
            userModel = new UserModel();
        }

        /// <summary>login</summary>
        /// <remarks>login</remarks>
        /// <returns></returns>
        /// <response code="200">return the new access token or annount already login</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">username or password is wrong</response>
        /// <response code="403">This account is enable to login</response>
        [HttpGet("/login")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        [ProducesResponseType(401, Type = typeof(ResponseFailType))]
        [ProducesResponseType(403, Type = typeof(ResponseFailType))]
        public async Task<object> Login([FromBody] UserLoginInfo info)
        {
            try
            {
                string token = Request.Headers["token"];
                if (token != "null") return Ok(Responder.Success("Already logined"));
                Result result = await userModel.Login(info.username, info.password);
                if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
                node1:
                IAuthContainerModel model = Helper.GetJWTContainerModel(info.username, info.password, result.data.ToString(), _jwtConfig);
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

        /// <summary>logout</summary>
        /// <remarks>logout</remarks>
        /// <returns></returns>
        /// <response code="200">reset access token</response>
        [HttpGet("/logout")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
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

        /// <summary>create new user</summary>
        /// <remarks>create new user</remarks>
        /// <param name="newUser">the information of new user you want to add in your database</param>
        /// <returns></returns>
        /// <response code="200">return infomation of new user</response>
        /// <response code="400">if get mistake</response>
        [HttpPost("/users")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        public async Task<object> CreateNewUser([FromBody] NewUserInfo newUser)
        {
            try
            {
                Result result = await userModel.InsertUser(newUser);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>get user by id</summary>
        /// <remarks>get user by id</remarks>
        /// <param name="userId">the id of user you want to get</param>
        /// <param name="fields">the specified fields you want to get</param>
        /// <returns></returns>
        /// <response code="200">return infomation of user with specified fields</response>
        /// <response code="400">if get mistake</response>
        [HttpGet("/users/{userId}")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        public async Task<object> GetUserById(string userId, [FromQuery] string fields)
        {
            try
            {
                Result result;
                if (fields != null)
                {
                    string[] fieldList = fields.Split(',');
                    result = await userModel.GetUserById(userId, fieldList);
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

        /// <summary>get list users</summary>
        /// <remarks>get list users</remarks>
        /// <param name="pageIndex">the page index you want to get</param>
        /// <param name="pageSize">the user per page you want to set</param>
        /// <returns></returns>
        /// <response code="200">return infomation of list user with pagination</response>
        /// <response code="400">if get mistake</response>
        [HttpGet("/users")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        public async Task<object> GetListUser([FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            try
            {
                Result result = await userModel.GetListUser();
                return Ok(Responder.Success(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>update user</summary>
        /// <remarks>update user</remarks>
        /// <returns></returns>
        /// <param name="updateUser">the info used to update</param>
        /// <param name="oldPassword">the confirm password to update</param>
        /// <param name="oldUsername">the confirm username to update</param>
        /// <response code="200">return infomation of user you updated</response>
        /// <response code="400">if get mistake</response>
        [HttpPut("/users")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        public async Task<object> UpdateUser([FromBody] UpdateUserInfo updateUser, 
            [FromQuery][Required] string oldUsername, [FromQuery][Required] string oldPassword)
        {
            try
            {
                string username = Request.Headers["username"];
                string password = Request.Headers["password"];
                if (oldUsername != username || oldPassword != password) return StatusCode(401, Responder.Fail("wrong username or password"));
                Result result = await userModel.UpdateUser(username, updateUser);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>update role of user</summary>
        /// <remarks>update role of user</remarks>
        /// <returns></returns>
        /// <param name="userId">the id of user you want to update</param>
        /// <param name="updateRoleUser">the info used to update role</param>
        /// <response code="200">return infomation of user you updated</response>
        /// <response code="400">if get mistake</response>
        /// <response code="422">Invalied argument</response>
        [HttpPut("/admin/users/{userId}")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        [ProducesResponseType(422, Type = typeof(ResponseFailType))]
        public async Task<object> UpdateRole(string userId, [FromBody] UpdateRoleUserInfo updateRoleUser)
        {
            try
            {
                string role = Request.Headers["role"];
                if (role == "user") return StatusCode(401, Responder.Fail("You not allow to update role"));
                Result result = await userModel.UpdateRole(userId, updateRoleUser);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        /// <summary>delete user</summary>
        /// <remarks>delete user</remarks>
        /// <returns></returns>
        /// <param name="userId">the id of user you want to delete</param>
        /// <response code="200">return infomation of user you deleted</response>
        /// <response code="400">if get mistake</response>
        [HttpDelete("/users/{userId}")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
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
