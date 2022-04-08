using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Passengers.API.Services;
using Passengers.API.Validators;

namespace Passengers.API.Controllers
{
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
        public async Task<ActionResult<List<Passenger>>> GetAll() =>
           await _passengerMongoService.Get();


        [HttpGet("{id:length(24)}", Name = "GetPassenger")]
        public async Task<ActionResult<Passenger>> Get(string id)
        {
            var passenger = await _passengerMongoService.Get(id);

            if (passenger == null)
                return NotFound("Passageiro não encontrado.");

            return passenger;
        }

        [HttpGet("{passaportNumber}")]
        public async Task<ActionResult<Passenger>> GetByPassaportNumber(string passaportNumber)
        {
            var passenger = await _passengerMongoService.GetByPassaportNumber(passaportNumber);

            if (passenger == null)
                return NotFound("Passageiro não encontrado.");

            return passenger;
        }

        [HttpPost]
        public async Task<ActionResult<Passenger>> Create(Passenger passenger)
        {
            (_, bool status) = await _passengerValidator.ValidateToCreate(passenger);

            if (!status)
                return BadRequest("Passageiro já registrado.");

            return CreatedAtRoute("GetPassenger", new { id = passenger.Id }, passenger);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Passenger passenger)
        {
            (_, bool status) = await _passengerValidator.ValidateToUpdate(id, passenger);

            if (!status)
                return BadRequest("Passageiro não encontrado.");

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            (_, bool status) = await _passengerValidator.ValidateToRemove(id);

            if (!status)
                return NotFound("Passageiro não encontrado.");

            return NoContent();
        }
    }
}
