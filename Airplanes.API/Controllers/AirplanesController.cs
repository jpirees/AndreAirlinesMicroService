using System.Collections.Generic;
using System.Threading.Tasks;
using Airplanes.API.Services;
using Airplanes.API.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Newtonsoft.Json;
using Utils.HttpApiResponse;
using Utils.Services;

namespace Airplanes.API.Controllers
{
    [EnableCors]
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
        [Authorize]
        public async Task<ActionResult<List<Airplane>>> GetAll() =>
           await _airplaneMongoService.Get();


        [HttpGet("{id:length(24)}", Name = "GetAirplane")]
        [Authorize]
        public async Task<ActionResult<Airplane>> Get(string id)
        {
            var airplane = await _airplaneMongoService.Get(id);

            if (airplane == null)
                return NotFound(new ApiResponse(404, "Aeronave não encontrada."));

            return airplane;
        }


        [HttpGet("{registrationCode}")]
        [Authorize]
        public async Task<ActionResult<Airplane>> GetByRegistrationCode(string registrationCode)
        {
            var airplane = await _airplaneMongoService.GetByRegistrationCode(registrationCode);

            if (airplane == null)
                return NotFound(new ApiResponse(404, "Aeronave não encontrada."));

            return airplane;
        }

        [HttpPost]
        [Authorize(Roles = "manager_airplanes")]
        public async Task<ActionResult<Airplane>> Create(Airplane airplane)
        {
            (_, var response) = await _airplaneValidator.ValidateToCreate(airplane);

            if (response.StatusCode.Equals(400))
                return BadRequest(response);

            var objectAfterJson = JsonConvert.SerializeObject(airplane).ToString();

            await LogAPIService.RegisterLog(new Log(null, null, objectAfterJson, "post", "airplanes"));

            return CreatedAtRoute("GetAirplane", new { id = airplane.Id }, airplane);
        }

        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "manager_airplanes")]
        public async Task<IActionResult> Update(string id, Airplane airplane)
        {
            var (airplaneOut, response) = await _airplaneValidator.ValidateToUpdate(id, airplane);

            if (response.StatusCode.Equals(404))
                return NotFound(response);

            var objectBeforeJson = JsonConvert.SerializeObject(airplaneOut).ToString();

            var objectAfterJson = JsonConvert.SerializeObject(airplane).ToString();

            await LogAPIService.RegisterLog(new Log(null, objectBeforeJson, objectAfterJson, "put", "airplanes"));

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "manager_airplanes")]
        public async Task<IActionResult> Delete(string id)
        {
            var (airplaneOut, response) = await _airplaneValidator.ValidateToRemove(id);

            if (response.StatusCode.Equals(404))
                return NotFound(response);

            var objectBeforeJson = JsonConvert.SerializeObject(airplaneOut).ToString();

            await LogAPIService.RegisterLog(new Log(null, objectBeforeJson, null, "delete", "airplanes"));

            return NoContent();
        }
    }
}
