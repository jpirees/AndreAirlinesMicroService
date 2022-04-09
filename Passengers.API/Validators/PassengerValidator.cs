using System.Threading.Tasks;
using Models.Entities;
using Passengers.API.Services;
using Utils.HttpApiResponse;
using Utils.Services;
using Utils.Validators;

namespace Passengers.API.Validators
{
    public class PassengerValidator
    {
        private readonly PassengerMongoService _passengerMongoService;

        public PassengerValidator(PassengerMongoService passengerMongoService)
        {
            _passengerMongoService = passengerMongoService;
        }

        public async Task<(Passenger, ApiResponse)> ValidateToCreate(Passenger passengerIn)
        {
            var isValid = ValidateDocument.IsValidDocument(passengerIn.Document);

            if (!isValid)
                return (passengerIn, new ApiResponse(400, "Número de documento inválido"));

            var passenger = await _passengerMongoService.GetByDocument(passengerIn.Document);

            if (passenger == null)
            {
                passenger = await _passengerMongoService.GetByPassaportNumber(passengerIn.PassaportNumber);

                if (passenger == null)
                {
                    var address = await ViaCepService.SearchAddressByZipCode(passengerIn.Address.ZipCode);

                    if (address != null)
                    {
                        address.Number = passengerIn.Address.Number;
                        passengerIn.Address = address;
                    }

                    await _passengerMongoService.Create(passengerIn);
                    return (passenger, new ApiResponse(201));
                }

                return (passenger, new ApiResponse(400, "Número de passaporte já cadastrado."));
            }

            return (passenger, new ApiResponse(400, "Passageiro já cadastrado."));
        }

        public async Task<(Passenger, ApiResponse)> ValidateToUpdate(string id, Passenger passengerIn)
        {
            var passenger = await _passengerMongoService.Get(id);

            if (passenger != null)
            {
                if (passengerIn.Address.ZipCode != passenger.Address.ZipCode)
                {
                    var address = await ViaCepService.SearchAddressByZipCode(passengerIn.Address.ZipCode);

                    if (address != null)
                    {
                        address.Number = passengerIn.Address.Number;
                        passengerIn.Address = address;
                    }
                }

                await _passengerMongoService.Update(id, passengerIn);

                return (passenger, new ApiResponse(200));
            }

            return (passenger, new ApiResponse(404, "Passageiro não encontrado."));
        }

        public async Task<(Passenger, ApiResponse)> ValidateToRemove(string id)
        {
            var passenger = await _passengerMongoService.Get(id);

            if (passenger == null)
                return (passenger, new ApiResponse(404, "Passageiro não encontrado."));

            await _passengerMongoService.Remove(id);

            return (passenger, new ApiResponse(200));
        }
    }
}
