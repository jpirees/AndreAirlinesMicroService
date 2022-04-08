using System.Threading.Tasks;
using Airplanes.API.Services;
using Models.Entities;

using Microsoft.AspNetCore.Mvc;

namespace Airplanes.API.Validators
{
    public class AirplaneValidator
    {
        private readonly AirplaneMongoService _airplaneMongoService;

        public AirplaneValidator(AirplaneMongoService airplaneMongoService)
        {
            _airplaneMongoService = airplaneMongoService;
        }

        public async Task<(Airplane, bool)> ValidateToCreate(Airplane airplaneIn)
        {
            var airplane = await _airplaneMongoService.GetByRegistrationCode(airplaneIn.RegistrationCode);

            if (airplane == null)
                await _airplaneMongoService.Create(airplaneIn);

            return (airplane, airplane == null);
        }

        public async Task<(Airplane, bool)> ValidateToUpdate(string id, Airplane airplaneIn)
        {
            var airplane = await _airplaneMongoService.Get(id);

            if (airplane != null)
                await _airplaneMongoService.Update(id, airplaneIn);

            return (airplane, airplane != null);
        }

        public async Task<(Airplane, bool)> ValidateToRemove(string id)
        {
            var airplane = await _airplaneMongoService.Get(id);

            if (airplane != null)
                await _airplaneMongoService.Remove(id);

            return (airplane, airplane != null);
        }
    }
}
