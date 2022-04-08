using System.Threading.Tasks;
using Models.Entities;
using Passengers.API.Services;
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

        public async Task<(Passenger, bool)> ValidateToCreate(Passenger passengerIn)
        {
            var isValid = ValidateDocument.IsValidDocument(passengerIn.Document);

            if (!isValid)
                return (null, isValid);

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
                }
            }

            return (passenger, passenger == null);
        }

        public async Task<(Passenger, bool)> ValidateToUpdate(string id, Passenger passengerIn)
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
            }

            return (passenger, passenger != null);
        }

        public async Task<(Passenger, bool)> ValidateToRemove(string id)
        {
            var passenger = await _passengerMongoService.Get(id);

            if (passenger != null)
                await _passengerMongoService.Remove(id);

            return (passenger, passenger != null);
        }

    }
}
