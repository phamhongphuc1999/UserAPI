using System.Security.Claims;

namespace UserAPI.Models.JWTModel
{
    public interface IAuthContainerModel
    {
        string SecretKey { get; }
        string SecurityAlgorithm { get; set; }
        int ExpireMinutes { get; }
        Claim[] Claims { get; set; }
    }
}
