using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UserAPI.Configuration;
using UserAPI.Services.JWTService;

namespace UserAPI.Controllers.SqlControllers
{
  public class BaseSqlController : ControllerBase
  {
    protected readonly IOptions<JWTConfig> _jwtConfig;
    protected IAuthService authService;

    public BaseSqlController(IOptions<JWTConfig> jwtConfig)
    {
      _jwtConfig = jwtConfig;
      authService = new JWTService(_jwtConfig.Value.SecretKey);
    }
  }
}
