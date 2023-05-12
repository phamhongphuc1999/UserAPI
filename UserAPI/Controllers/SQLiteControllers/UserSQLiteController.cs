using System;
using System.Linq;
using UserAPI.Services;
using UserAPI.Connector;
using UserAPI.Configuration;
using UserAPI.Models.JWTModel;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Models.CommonModel;
using UserAPI.Models.SQLiteModel;
using UserAPI.Services.JWTService;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using JwtHelper = UserAPI.Models.JWTModel.Helper;

namespace UserAPI.Controllers.SQLiteControllers
{
  [Produces("application/json")]
  [Consumes("application/json")]
  [ApiController]
  public class UserSQLiteController : BaseSQLiteController
  {
    public UserSQLiteController(IOptions<JWTConfig> jwtConfig) : base(jwtConfig)
    {
    }

    [HttpPost("/sqlite/user")]
    [ProducesResponseType(201, Type = typeof(ResponseSuccessType))]
    [ProducesResponseType(400, Type = typeof(ResponseFailType))]
    public ObjectResult CreateNewUser([FromBody] SQLiteUser user)
    {
      try
      {
        Result result = ServiceSelector.userSQLiteService.InsertUser(user, APIConnection.SQLite);
        if (result.status == Status.Created) return Ok(Responder.Success(result.data));
        else return StatusCode(result.status, Responder.Fail(result.data));
      }
      catch (Exception error)
      {
        return BadRequest(Responder.Fail(error.Message));
      }
    }

    [HttpPost("/sqlite/login")]
    [ProducesResponseType(200, Type = typeof(ResponseSuccessType))]
    [ProducesResponseType(400, Type = typeof(ResponseFailType))]
    [ProducesResponseType(401, Type = typeof(ResponseFailType))]
    [ProducesResponseType(403, Type = typeof(ResponseFailType))]
    public ObjectResult Login([FromBody] LoginUser loginUser)
    {
      try
      {
        StringValues token;
        Request.Headers.TryGetValue("token", out token);
        string _token = token.FirstOrDefault();
        if (Utilities.IsValidToken(_token)) return Ok(Responder.Success("Already login"));
        Result result = ServiceSelector.userSQLiteService.Login(loginUser.Username, loginUser.Password, APIConnection.SQLite);
        if (result.status != 200) return StatusCode(result.status, Responder.Fail(result.data));
        node1:
        HelperSQLiteTokenUser user = (HelperSQLiteTokenUser)result.data;
        IAuthContainerModel model = JwtHelper.GetJWTContainerModel1(user.userId, user.Username, _jwtConfig);
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

    [HttpGet("/sqlite/users")]
    public ObjectResult GetListUsers([FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] string fields)
    {
      try
      {
        Result result = new Result();
        return Ok(Responder.Success(result.data));
      }
      catch (Exception error)
      {
        return BadRequest(Responder.Fail(error.Message));
      }
    }
  }
}
