using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace UserAPI.Models.JWTModel
{
    public static class Helper
    {
        public static JWTContainerModel GetJWTContainerModel(string username, string password, string role, IOptions<JWTConfig> config)
        {
            return new JWTContainerModel(config)
            {
                Claims = new Claim[]
                {
                    new Claim("Password", password),
                    new Claim(ClaimTypes.Name, username),
                    new Claim("Role", role)
                }
            };
        }
    }
}
