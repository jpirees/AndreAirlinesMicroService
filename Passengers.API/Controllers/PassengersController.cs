using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Passengers.API.Services;
using Passengers.API.Validators;
using Utils.HttpApiResponse;

namespace Passengers.API.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {
        private readonly PassengerMongoService _passengerMongoService;
        private readonly PassengerValidator _passengerValidator;

        public PassengersController(PassengerMongoService passengerMongoService, PassengerValidator passengerValidator)
        {
            _passengerMongoService = passengerMongoService;
            _passengerValidator = passengerValidator;
        }

        [HttpGet]
        [Authorize(Roles = "manager_passengers")]
        public async Task<ActionResult<List<Passenger>>> GetAll() =>
           await _passengerMongoService.Get();


        [HttpGet("{id:length(24)}", Name = "GetPassenger")]
        [Authorize(Roles = "manager_passengers,clerk_passengers")]
        public async Task<ActionResult<Passenger>> Get(string id)
        {
            var passenger = await _passengerMongoService.Get(id);

            if (passenger == null)
                return NotFound(new ApiResponse(404, "Passageiro não encontrado."));

            return passenger;
        }

        [HttpGet("{passaportNumber}")]
        [Authorize(Roles = "manager_passengers,clerk_passengers")]
        public async Task<ActionResult<Passenger>> GetByPassaportNumber(string passaportNumber)
        {
            var passenger = await _passengerMongoService.GetByPassaportNumber(passaportNumber);

            if (passenger == null)
                return NotFound(new ApiResponse(404, "Passageiro não encontrado."));

            return passenger;
        }

        [HttpPost]
        [Authorize(Roles = "manager_passengers,clerk_passengers")]
        public async Task<ActionResult<Passenger>> Create(Passenger passenger)
        {
            (_, var response) = await _passengerValidator.ValidateToCreate(passenger);

            if (response.StatusCode.Equals(400))
                return BadRequest(response);

            return CreatedAtRoute("GetPassenger", new { id = passenger.Id }, passenger);
        }

        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "manager_passengers,clerk_passengers")]
        public async Task<IActionResult> Update(string id, Passenger passenger)
        {
            (_, var response) = await _passengerValidator.ValidateToUpdate(id, passenger);

            if (response.StatusCode.Equals(404))
                return NotFound(response);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "manager_passengers")]
        public async Task<IActionResult> Delete(string id)
        {
            (_, var response) = await _passengerValidator.ValidateToRemove(id);

            if (response.StatusCode.Equals(404))
                return NotFound(response);

            return NoContent();
        }
    }
}
