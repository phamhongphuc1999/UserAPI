using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UserAPI.Models.MongoModel;
using UserAPI.Models.JWTModel;
using UserAPI.Services.JWTService;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using UserAPI.Models.CommonModel;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Security.Claims;
using UserAPI.Configuration;
using UserAPI.Services;
using JwtHelper = UserAPI.Models.JWTModel.Helper;

namespace UserAPI.Controllers.MongoControllers
{
  [ApiController]
  [Produces("application/json")]
  [Consumes("application/json")]
  public class UserController : BaseMongoController
  {
    public UserController(IOptions<JWTConfig> jwtConfig) : base(jwtConfig)
    {
    }

    /// <summary>Login</summary>
    /// <remarks>Login</remarks>
    /// <returns>The JWT token and user information</returns>
    /// <response code="200">New JWT token or the announcement that you already login</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Username or password is incorrect</response>
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
        if (Utilities.IsValidToken(_token)) return Ok(Responder.Success("Already login"));
        Result result = await ServiceSelector.Mongo.user.Login(info.username, info.password);
        if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
        node1:
        HelperTokenUser user = (HelperTokenUser)result.data;
        IAuthContainerModel model = JwtHelper.GetJWTContainerModel(user.userId, user.username, user.email, _jwtConfig);
        IAuthService authService = new JWTService(model.SecretKey);
        string accessToken = authService.GenerateToken(model);
        if (!authService.IsTokenValid(accessToken)) goto node1;
        return Ok(Responder.Success(new
        {
          token = accessToken,
          user = result.data
        }));
      }
      catch (Exception error)
      {
        return BadRequest(Responder.Fail(error.Message));
      }
    }

#pragma warning disable CS1998
    /// <summary>Logout</summary>
    /// <remarks>Logout</remarks>
    /// <returns></returns>
    /// <response code="200">Reset access token</response>
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

    /// <summary>Create new user</summary>
    /// <remarks>Create new user</remarks>
    /// <param name="newUser">The information of new user you want to add in your database</param>
    /// <returns></returns>
    /// <response code="200">Return information of new user</response>
    /// <response code="400">Return error when it occur</response>
    [HttpPost("/register")]
    [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
    [ProducesResponseType(400, Type = typeof(ResponseFailType))]
    public async Task<object> Register([FromBody] NewUserInfo newUser)
    {
      try
      {
        Result result = await ServiceSelector.Mongo.user.Register(newUser);
        if (result.status == 200) return Ok(Responder.Success(result.data));
        else return StatusCode(result.status, Responder.Fail(result.data));
      }
      catch (Exception error)
      {
        return BadRequest(Responder.Fail(error.Message));
      }
    }

    /// <summary>Get user by id</summary>
    /// <remarks>Get user by id</remarks>
    /// <param name="userId">The id of user you want to get</param>
    /// <param name="fields">The specified fields you want to get</param>
    /// <returns></returns>
    /// <response code="200">Return information of user with specified fields</response>
    /// <response code="400">Return error when it occur</response>
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
          string[] fieldList = Utilities.SplitFields(fields);
          result = await ServiceSelector.Mongo.user.GetUserById(userId, fieldList);
        }
        else result = await ServiceSelector.Mongo.user.GetUserById(userId);
        if (result.status == 200) return Ok(Responder.Success(result.data));
        return StatusCode(result.status, Responder.Fail(result.data));
      }
      catch (Exception error)
      {
        return BadRequest(Responder.Fail(error.Message));
      }
    }

    /// <summary>Get current user information</summary>
    /// <remarks>Get current user information</remarks>
    /// <returns>The current user information</returns>
    /// <response code="200">The current user information</response>
    /// <response code="400">Return error when it occur</response>
    [HttpGet("/users/current-user")]
    [CustomAuthorization]
    [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
    [ProducesResponseType(400, Type = typeof(ResponseFailType))]
    public async Task<object> GetCurrentUser()
    {
      try
      {
        string token = HttpContext.Request.Headers["token"];
        List<Claim> claims = authService.GetTokenClaims(token).ToList();
        string username = claims.Find(x => x.Type == ClaimTypes.Name).Value;
        Result result = await ServiceSelector.Mongo.user.GetUserByUserName(username, new string[] { "username" });
        if (result.status == 200) return Ok(Responder.Success(result.data));
        else return StatusCode(result.status, Responder.Fail(result.data));
      }
      catch (Exception error)
      {
        return BadRequest(Responder.Fail(error.Message));
      }
    }

    /// <summary>Get list users</summary>
    /// <remarks>Get list users</remarks>
    /// <param name="pageIndex">The page index you want to get</param>
    /// <param name="pageSize">The user per page you want to set</param>
    /// <param name="fields">The specified fields you want to get</param>
    /// <returns></returns>
    /// <response code="200">Return information of list user with pagination</response>
    /// <response code="400">Return error when it occur</response>
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
          string[] fieldList = Utilities.SplitFields(fields);
          result = await ServiceSelector.Mongo.user.GetListUser(pageSize, pageIndex, fieldList);
        }
        else result = await ServiceSelector.Mongo.user.GetListUser(pageSize, pageIndex);
        if (result.status == 200) return Ok(Responder.Success(result.data));
        else return StatusCode(result.status, Responder.Fail(result.data));
      }
      catch (Exception error)
      {
        return BadRequest(Responder.Fail(error.Message));
      }
    }

    /// <summary>Update user</summary>
    /// <remarks>Update user</remarks>
    /// <returns></returns>
    /// <param name="updateUser">The info used to update</param>
    /// <param name="oldPassword">The confirm password to update</param>
    /// <param name="oldUsername">The confirm username to update</param>
    /// <response code="200">Return information of user you updated</response>
    /// <response code="400">Return error when it occur</response>
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
        Result result = await ServiceSelector.Mongo.user.UpdateUser(username, updateUser);
        if (result.status == 200) return Ok(Responder.Success(result.data));
        else return StatusCode(result.status, Responder.Fail(result.data));
      }
      catch (Exception error)
      {
        return BadRequest(Responder.Fail(error.Message));
      }
    }

    /// <summary>Delete user</summary>
    /// <remarks>Delete user</remarks>
    /// <returns></returns>
    /// <response code="200">Return information of user you deleted</response>
    /// <response code="400">Return error when it occur</response>
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
        Result result = await ServiceSelector.Mongo.user.DeleteUser(username);
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
