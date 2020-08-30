using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace UserAPI.Models
{
    public class JWTContainerModel: IAuthContainerModel
    {
        private readonly IOptions<JWTConfig> _config;

        public JWTContainerModel(IOptions<JWTConfig> config)
        {
            _config = config;
        }

        public int ExpireMinutes { get { return _config.Value.ExpireMinutes; } }
        public string SecretKey { get { return _config.Value.SecretKey; } }
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
        public Claim[] Claims { get; set; }
    }
}
