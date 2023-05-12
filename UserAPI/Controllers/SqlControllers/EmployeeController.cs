using System;
using System.Linq;
using UserAPI.Services;
using UserAPI.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Models.MySqlModel;
using UserAPI.Models.CommonModel;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace UserAPI.Controllers.SqlControllers
{
  public class EmployeeController : BaseSqlController
  {
    public EmployeeController(IOptions<JWTConfig> jwtConfig) : base(jwtConfig) { }

    /// <summary>Login</summary>
    /// <remarks>Login</remarks>
    /// <returns>The JWT token and user information</returns>
    /// <response code="200">New JWT token or the announcement that you already login</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Username or password is incorrect</response>
    /// <response code="403">This account is enable to login</response>
    [HttpPost("/sql/user")]
    [ProducesResponseType(201, Type = typeof(ResponseSuccessType))]
    [ProducesResponseType(400, Type = typeof(ResponseFailType))]
    public ObjectResult Login([FromBody] LoginEmployeeInfo user)
    {
      try
      {
        StringValues token;
        Request.Headers.TryGetValue("token", out token);
        string _token = token.FirstOrDefault();
        if (Utilities.IsValidToken(_token)) return Ok(Responder.Success("Already login"));
        Result result = ServiceSelector.employeeService.Login(user.Username, user.Password);
        if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
        node1:
        Models.MongoModel.HelperTokenUser _user = (Models.MongoModel.HelperTokenUser)result.data;
        Models.JWTModel.IAuthContainerModel model = Models.JWTModel.Helper.GetJWTContainerModel(_user.userId, _user.username, _user.email, _jwtConfig);
        Services.JWTService.IAuthService authService = new Services.JWTService.JWTService(model.SecretKey);
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

    /// <summary>Get current user information</summary>
    /// <remarks>Get current user information</remarks>
    /// <returns>The current user information</returns>
    /// <response code="200">The current user information</response>
    /// <response code="400">Bad request</response>
    [HttpGet("/sql/users/current-user")]
    [CustomAuthorization]
    [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
    [ProducesResponseType(400, Type = typeof(ResponseFailType))]
    public ObjectResult GetCurrentUser()
    {
      try
      {
        string token = HttpContext.Request.Headers["token"];
        List<Claim> claims = authService.GetTokenClaims(token).ToList();
        string username = claims.Find(x => x.Type == ClaimTypes.Name).Value;
        Result result = ServiceSelector.employeeService.GetEmployeeByUsername(username);
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
