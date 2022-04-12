using System.Threading.Tasks;
using Models.Entities;
using Models.Entities.DTO;
using Utils.HttpApiResponse;
using Utils.Services;

namespace Authentications.API.Services
{
    public class AuthenticationService
    {
        public static async Task<(UserResponseDTO, ApiResponse)> GetUserAsync(UserRequestDTO userIn)
        {
            var user = await UserAPIService.SearchByUsername(userIn.Username, userIn.Password);

            if (user == null)
                return (null, new ApiResponse(404, "Usuário não encontrado"));

            return (user, new ApiResponse(200));
        }
    }
}
