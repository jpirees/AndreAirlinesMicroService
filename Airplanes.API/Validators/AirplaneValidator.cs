using System.Threading.Tasks;
using Airplanes.API.Services;
using Models.Entities;

using Microsoft.AspNetCore.Mvc;
using Utils.HttpApiResponse;

namespace Airplanes.API.Validators
{
    public class AirplaneValidator
    {
        private readonly AirplaneMongoService _airplaneMongoService;

        public AirplaneValidator(AirplaneMongoService airplaneMongoService)
        {
            _airplaneMongoService = airplaneMongoService;
        }

        public async Task<(Airplane, ApiResponse)> ValidateToCreate(Airplane airplaneIn)
        {
            var airplane = await _airplaneMongoService.GetByRegistrationCode(airplaneIn.RegistrationCode);

            if (airplane != null)
                return (airplane, new ApiResponse(400, "Aeronave já registrada."));

            await _airplaneMongoService.Create(airplaneIn);

            return (airplane, new ApiResponse(201));
        }

        public async Task<(Airplane, ApiResponse)> ValidateToUpdate(string id, Airplane airplaneIn)
        {
            var airplane = await _airplaneMongoService.Get(id);

            if (airplane == null)
                return (airplane, new ApiResponse(404, "Aeronave não encontrada."));

            await _airplaneMongoService.Update(id, airplaneIn);

            return (airplane, new ApiResponse(204));
        }

        public async Task<(Airplane, ApiResponse)> ValidateToRemove(string id)
        {
            var airplane = await _airplaneMongoService.Get(id);

            if (airplane == null)
                return (airplane, new ApiResponse(404, "Aeronave não encontrada."));

            await _airplaneMongoService.Remove(id);

            return (airplane, new ApiResponse(204));
        }
    }
}
