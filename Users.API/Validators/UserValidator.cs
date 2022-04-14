using System.Threading.Tasks;
using Models.Entities;
using Users.API.Services;
using Utils.HttpApiResponse;
using Utils.Services;
using Utils.Validators;

namespace Users.API.Validators
{
    public class UserValidator
    {
        private readonly UserMongoService _userMongoService;

        public UserValidator(UserMongoService passengerMongoService)
        {
            _userMongoService = passengerMongoService;
        }

        public async Task<(User, ApiResponse)> ValidateToCreate(User userIn)
        {
            var isValid = ValidateDocument.IsValidDocument(userIn.Document);

            if (!isValid)
                return (userIn, new ApiResponse(400, "Número de documento inválido"));

            var user = await _userMongoService.GetByDocument(userIn.Document);

            if (user == null)
            {
                user = await _userMongoService.GetByUsername(userIn.Username);

                if (user == null)
                {
                    var address = await ViaCepService.SearchAddressByZipCode(userIn.Address.ZipCode);

                    if (address != null)
                    {
                        address.Number = userIn.Address.Number;
                        userIn.Address = address;
                    }

                    userIn = await _userMongoService.Create(userIn);
                    return (userIn, new ApiResponse(201));
                }

                return (user, new ApiResponse(400, "Login de usuário já cadastrado."));
            }

            return (user, new ApiResponse(400, "Usuário já cadastrado."));
        }

        public async Task<(User, ApiResponse)> ValidateToUpdate(string id, User userIn)
        {
            var user = await _userMongoService.Get(id);

            if (user != null)
            {
                if (userIn.Address.ZipCode != user.Address.ZipCode)
                {
                    var address = await ViaCepService.SearchAddressByZipCode(userIn.Address.ZipCode);

                    if (address != null)
                    {
                        address.Number = userIn.Address.Number;
                        userIn.Address = address;
                    }
                }

                await _userMongoService.Update(id, userIn);

                user.Password = null;

                return (user, new ApiResponse(200));
            }

            return (user, new ApiResponse(404, "Usuário não encontrado."));
        }

        public async Task<(User, ApiResponse)> ValidateToRemove(string id)
        {
            var user = await _userMongoService.Get(id);

            if (user == null)
                return (user, new ApiResponse(404, "Usuário não encontrado."));

            await _userMongoService.Remove(id);

            return (user, new ApiResponse(200));
        }
    }
}
