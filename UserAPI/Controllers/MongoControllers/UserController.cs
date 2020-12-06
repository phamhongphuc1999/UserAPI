// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using System;
using UserAPI.Services.MongoService;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UserAPI.Models.MongoModel;
using UserAPI.Models.JWTModel;
using UserAPI.Services.JWTService;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using UserAPI.Models.CommonModel;
using UserAPI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Security.Claims;

namespace UserAPI.Controllers.MongoControllers
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IOptions<JWTConfig> _jwtConfig;
        private readonly ILogger<UserController> _logger;
        private IAuthService authService;
        private UserService userService;

        public UserController(IOptions<JWTConfig> jwtConfig, ILogger<UserController> _logger)
        {
            this._logger = _logger;
            _jwtConfig = jwtConfig;
            userService = new UserService("MoneyLover", "User");
            authService = new JWTService(_jwtConfig.Value.SecretKey);
        }

        /// <summary>login</summary>
        /// <remarks>login</remarks>
        /// <returns></returns>
        /// <response code="200">return the new access token or annount already login</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">username or password is wrong</response>
        /// <response code="403">This account is enable to login</response>
        [HttpPost("/login")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        [ProducesResponseType(401, Type = typeof(ResponseFailType))]
        [ProducesResponseType(403, Type = typeof(ResponseFailType))]
        public async Task<object> Login([FromBody] UserLoginInfo info)
        {
            try
            {
                StringValues token;
                Request.Headers.TryGetValue("token", out token);
                string _token = token.FirstOrDefault();
                if (_token != null && _token != "") return Ok(Responder.Success("Already logined"));
                Result result = await userService.LoginAsync(info.username, info.password);
                if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
                node1:
                IAuthContainerModel model = Helper.GetJWTContainerModel(info.username, info.password, _jwtConfig);
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

#pragma warning disable CS1998
        /// <summary>logout</summary>
        /// <remarks>logout</remarks>
        /// <returns></returns>
        /// <response code="200">reset access token</response>
        [HttpPost("/logout")]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        public async Task<object> Logout()
        {
            try
            {
                StringValues token;
                Request.Headers.TryGetValue("token", out token);
                string _token = token.FirstOrDefault();
                if (_token == null || _token == "") return Ok(Responder.Success("Already logout"));
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
                Result result = await userService.InsertUserAsync(newUser);
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
        [CustomAuthorization]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        public async Task<object> GetUserById(string userId, [FromQuery] string fields)
        {
            try
            {
                Result result;
                if (fields != null)
                {
                    string[] fieldList = HelperService.SplipFields(fields);
                    result = await userService.GetUserByIdAsync(userId, fieldList);
                }
                else result = await userService.GetUserByIdAsync(userId);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                return StatusCode(result.status, Responder.Fail(result.data));
            }
            catch (Exception error)
            {
                return BadRequest(Responder.Fail(error.Message));
            }
        }

        [HttpGet("/users/current-user")]
        [CustomAuthorization]
        public async Task<object> GetCurrentUser()
        {
            try
            {
                string token = HttpContext.Request.Headers["token"];
                List<Claim> claims = authService.GetTokenClaims(token).ToList();
                string username = claims.Find(x => x.Type == ClaimTypes.Name).Value;
                Result result = await userService.GetUserByUserNameAsync(username);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
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
        /// <param name="fields">the specified fields you want to get</param>
        /// <returns></returns>
        /// <response code="200">return infomation of list user with pagination</response>
        /// <response code="400">if get mistake</response>
        [HttpGet("/users")]
        [CustomAuthorization]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        public async Task<object> GetListUser([FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] string fields)
        {
            try
            {
                Result result;
                if (fields != null)
                {
                    string[] fieldList = HelperService.SplipFields(fields);
                    result = await userService.GetListUserAsync(pageSize, pageIndex, fieldList);
                }
                else result = await userService.GetListUserAsync(pageSize, pageIndex);
                if (result.status == 200) return Ok(Responder.Success(result.data));
                else return StatusCode(result.status, Responder.Fail(result.data));
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
        [CustomAuthorization]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        public async Task<object> UpdateUser([FromBody] UpdateUserInfo updateUser,
            [FromQuery][Required] string oldUsername, [FromQuery][Required] string oldPassword)
        {
            try
            {
                string token = HttpContext.Request.Headers["token"];
                List<Claim> claims = authService.GetTokenClaims(token).ToList();
                string username = claims.Find(x => x.Type == ClaimTypes.Name).Value;
                string password = claims.Find(x => x.Type == "Password").Value;
                if (oldUsername != username || oldPassword != password) return StatusCode(401, Responder.Fail("wrong username or password"));
                Result result = await userService.UpdateUserAsync(username, updateUser);
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
        /// <response code="200">return infomation of user you deleted</response>
        /// <response code="400">if get mistake</response>
        /// <response code="401">You not allow to action</response>
        [HttpDelete("/users")]
        [CustomAuthorization]
        [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
        [ProducesResponseType(400, Type = typeof(ResponseFailType))]
        [ProducesResponseType(401, Type = typeof(ResponseFailType))]
        public async Task<object> DeleteUser()
        {
            try
            {
                string token = HttpContext.Request.Headers["token"];
                List<Claim> claims = authService.GetTokenClaims(token).ToList();
                string username = claims.Find(x => x.Type == ClaimTypes.Name).Value;
                Result result = await userService.DeleteUserAsync(username);
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
