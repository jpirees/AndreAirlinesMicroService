using System.Collections.Generic;
using System.Threading.Tasks;
using Flights.API.Services;
using Flights.API.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Utils.HttpApiResponse;

namespace Flights.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly FlightMongoService _flightMongoService;
        private readonly FlightValidator _flightValidator;

        public FlightsController(FlightMongoService flightMongoService, FlightValidator flightValidator)
        {
            _flightMongoService = flightMongoService;
            _flightValidator = flightValidator;
        }

        [HttpGet]
        //public async Task<ActionResult<List<Flight>>> GetAll() =>
        //   await _flightMongoService.Get();

        public async Task<ActionResult<List<Flight>>> GetFlights(string? from = null, string? to = null)
        {
            if (string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
                return await _flightMongoService.Get();

            else if (!string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
                return await _flightMongoService.GetByAirportOrigin(from);

            else if (string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
                return await _flightMongoService.GetByAirportDestination(to);

            else
               return await _flightMongoService.GetByFlightFromTo(from, to);
        }


        [HttpGet("{id:length(24)}", Name = "GetFlight")]
        public async Task<ActionResult<Flight>> Get(string id)
        {
            var flight = await _flightMongoService.Get(id);

            if (flight == null)
                return NotFound(new ApiResponse(404, "Vôo não encontrado."));

            return flight;
        }

        [HttpPost]
        public async Task<ActionResult<Flight>> Create(Flight flight)
        {
            (_, var response) = await _flightValidator.ValidateToCreate(flight);

            return response.StatusCode switch
            {
                400 => BadRequest(response),
                404 => NotFound(response),
                _ => CreatedAtRoute("GetFlight", new { id = flight.Id }, flight)
            };
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Flight flight)
        {
            (_, var response) = await _flightValidator.ValidateToUpdate(id, flight);

            return response.StatusCode switch
            {
                400 => BadRequest(response),
                404 => NotFound(response),
                _ => NoContent()
            };
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            (_, var response) = await _flightValidator.ValidateToRemove(id);

            if (response.StatusCode.Equals(404))
                return NotFound(response);

            return NoContent();
        }
    }
}
