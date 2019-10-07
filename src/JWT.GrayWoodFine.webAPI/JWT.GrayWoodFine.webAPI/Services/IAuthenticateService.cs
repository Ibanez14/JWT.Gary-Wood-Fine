using JWT.GrayWoodFine.webAPI.Models;

namespace JWT.GrayWoodFine.webAPI.Services
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(TokenRequest request, out string token);
    }
}
