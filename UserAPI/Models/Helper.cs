using System.Security.Claims;

namespace UserAPI.Models
{
    public static class Helper
    {
        public static JWTContainerModel GetJWTContainerModel(string username, string password)
        {
            return new JWTContainerModel()
            {
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, password),
                    new Claim(ClaimTypes.Name, username)
                }
            };
        }
    }
}
