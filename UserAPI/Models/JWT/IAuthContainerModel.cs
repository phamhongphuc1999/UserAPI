using System.Security.Claims;

namespace UserAPI.Models.JWT
{
    public interface IAuthContainerModel
    {
        string SecretKey { get; }
        string SecurityAlgorithm { get; set; }
        int ExpireMinutes { get; }
        Claim[] Claims { get; set; }
    }
}
