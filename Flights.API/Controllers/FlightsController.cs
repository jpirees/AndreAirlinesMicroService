using System.Collections.Generic;
using System.Threading.Tasks;
using Flights.API.Services;
using Flights.API.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Newtonsoft.Json;
using Utils.HttpApiResponse;
using Utils.Services;

namespace Flights.API.Controllers
{
    [EnableCors]
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
        [Authorize]
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
        [Authorize]
        public async Task<ActionResult<Flight>> Get(string id)
        {
            var flight = await _flightMongoService.Get(id);

            if (flight == null)
                return NotFound(new ApiResponse(404, "Vôo não encontrado."));

            return flight;
        }

        [HttpPost]
        [Authorize(Roles = "manager_flights")]
        public async Task<ActionResult<Flight>> Create(Flight flight)
        {
            var (flightOut, response) = await _flightValidator.ValidateToCreate(flight);

            switch (response.StatusCode)
            {
                case 400:
                    return BadRequest(response);

                case 404:
                    return NotFound(response);

                default:
                    var objectAfterJson = JsonConvert.SerializeObject(flight).ToString();

                    await LogAPIService.RegisterLog(new Log(null, null, objectAfterJson, "post", "flights"));

                    return CreatedAtRoute("GetFlight", new { id = flight.Id }, flight);
            }
        }

        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "manager_flights")]
        public async Task<IActionResult> Update(string id, Flight flight)
        {
            var (flightOut,  response) = await _flightValidator.ValidateToUpdate(id, flight);

            switch (response.StatusCode)
            {
                case 400:
                    return BadRequest(response);

                case 404:
                    return NotFound(response);

                default:
                    var objectBeforeJson = JsonConvert.SerializeObject(flightOut).ToString();

                    var objectAfterJson = JsonConvert.SerializeObject(flight).ToString();

                    await LogAPIService.RegisterLog(new Log(null, objectBeforeJson, objectAfterJson, "put", "flights"));

                    return NoContent();
            }
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "manager_flights")]
        public async Task<IActionResult> Delete(string id)
        {
            var (flightOut, response) = await _flightValidator.ValidateToRemove(id);

            if (response.StatusCode.Equals(404))
                return NotFound(response);

            var objectBeforeJson = JsonConvert.SerializeObject(flightOut).ToString();

            await LogAPIService.RegisterLog(new Log(null, objectBeforeJson, null, "delete", "flights"));


            return NoContent();
        }
    }
}
