using System.Collections.Generic;
using System.Threading.Tasks;
using Airplanes.API.Services;
using Airplanes.API.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Utils.HttpApiResponse;

namespace Airplanes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirplanesController : ControllerBase
    {
        private readonly AirplaneMongoService _airplaneMongoService;
        private readonly AirplaneValidator _airplaneValidator;

        public AirplanesController(AirplaneMongoService airplaneMongoService, AirplaneValidator airplaneValidator)
        {
            _airplaneMongoService = airplaneMongoService;
            _airplaneValidator = airplaneValidator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Airplane>>> GetAll() =>
           await _airplaneMongoService.Get();


        [HttpGet("{id:length(24)}", Name = "GetAirplane")]
        public async Task<ActionResult<Airplane>> Get(string id)
        {
            var airplane = await _airplaneMongoService.Get(id);

            if (airplane == null)
                return NotFound(new ApiResponse(404, "Aeronave não encontrada."));

            return airplane;
        }


        [HttpGet("{registrationCode}")]
        public async Task<ActionResult<Airplane>> GetByRegistrationCode(string registrationCode)
        {
            var airplane = await _airplaneMongoService.GetByRegistrationCode(registrationCode);

            if (airplane == null)
                return NotFound(new ApiResponse(404, "Aeronave não encontrada."));

            return airplane;
        }

        [HttpPost]
        public async Task<ActionResult<Airplane>> Create(Airplane airplane)
        {
            (_, var response) = await _airplaneValidator.ValidateToCreate(airplane);

            if (response.StatusCode.Equals(400))
                return BadRequest(response);

            return CreatedAtRoute("GetAirplane", new { id = airplane.Id }, airplane);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Airplane airplane)
        {
            (_, var response) = await _airplaneValidator.ValidateToUpdate(id, airplane);

            if (response.StatusCode.Equals(404))
                return NotFound(response);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            (_, var response) = await _airplaneValidator.ValidateToRemove(id);


            if (response.StatusCode.Equals(404))
                return NotFound(response);

            return NoContent();
        }
    }
}
