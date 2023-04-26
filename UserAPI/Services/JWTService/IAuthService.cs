using System.Security.Claims;
using System.Collections.Generic;
using UserAPI.Models.JWTModel;

namespace UserAPI.Services.JWTService
{
  public interface IAuthService
  {
    string SecretKey { get; set; }
    bool IsTokenValid(string token);
    string GenerateToken(IAuthContainerModel model);
    IEnumerable<Claim> GetTokenClaims(string token);
  }
}
