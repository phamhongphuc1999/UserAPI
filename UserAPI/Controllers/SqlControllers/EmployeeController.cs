using Microsoft.AspNetCore.Mvc;
using UserAPI.Models.MySqlModel;
using System;
using UserAPI.Models.CommonModel;
using UserAPI.Services;
using Microsoft.Extensions.Primitives;
using System.Linq;
using Microsoft.Extensions.Options;
using UserAPI.Configuration;

namespace UserAPI.Controllers.SqlControllers
{
  public class EmployeeController : BaseSqlController
  {
    public EmployeeController(IOptions<JWTConfig> jwtConfig) : base(jwtConfig)
    {
    }

    /// <summary>login</summary>
    /// <remarks>login</remarks>
    /// <returns>The token and basic information of user</returns>
    /// <response code="200">return the new access token or announce already login</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">username or password is wrong</response>
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
  }
}