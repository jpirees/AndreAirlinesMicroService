using System.Threading.Tasks;
using Flights.API.Services;
using Models.Entities;
using Utils.HttpApiResponse;

namespace Flights.API.Validators
{
    public class FlightValidator
    {
        private readonly FlightMongoService _flightMongoService;

        public FlightValidator(FlightMongoService flightMongoService)
        {
            _flightMongoService = flightMongoService;
        }

        public async Task<(Flight, ApiResponse)> ValidateToCreate(Flight flightIn)
        {
            var airplane = await AirplaneAPIService.SearchByAirplane(flightIn.Airplane.RegistrationCode);

            if (airplane == null)
                return (flightIn, new ApiResponse(404, "Aeronave não encontrada."));


            var airportOrigin = await AirportAPIService.SearchByAirport(flightIn.AirportOrigin.Code);

            if (airportOrigin == null)
                return (flightIn, new ApiResponse(404, "Aeroporto de origem não encontrado."));


            var airportDestination = await AirportAPIService.SearchByAirport(flightIn.AirportDestination.Code);

            if (airportDestination == null)
                return (flightIn, new ApiResponse(404, "Aeroporto de destino não encontrado."));


            var flight = await _flightMongoService.GetByFlight(airplane.RegistrationCode, airportOrigin.Code, airportDestination.Code);

            if (flight != null && flightIn.BoardingTime.Date.Equals(flight.BoardingTime.Date))
                return (flight, new ApiResponse(400, "Vôo já cadastrado."));

            flightIn.Airplane = airplane;
            flightIn.AirportOrigin = airportOrigin;
            flightIn.AirportDestination = airportDestination;

            await _flightMongoService.Create(flightIn);

            return (flight, new ApiResponse(201));
        }

        public async Task<(Flight, ApiResponse)> ValidateToUpdate(string id, Flight flightIn)
        {
            var flight = await _flightMongoService.Get(id);

            if (flight != null)
            {
                if (!flight.Airplane.RegistrationCode.Equals(flightIn.Airplane.RegistrationCode))
                {
                    var airplane = await AirplaneAPIService.SearchByAirplane(flightIn.Airplane.RegistrationCode);

                    if (airplane == null)
                        return (flightIn, new ApiResponse(404, "Aeronave não encontrada."));

                    flightIn.Airplane = airplane;
                }

                if (!flight.AirportOrigin.Code.Equals(flightIn.AirportOrigin.Code))
                {
                    var airportOrigin = await AirportAPIService.SearchByAirport(flightIn.AirportOrigin.Code);

                    if (airportOrigin == null)
                        return (flightIn, new ApiResponse(404, "Aeroporto de origem não encontrado."));

                    flightIn.AirportOrigin = airportOrigin;
                }

                if (!flight.AirportDestination.Code.Equals(flightIn.AirportDestination.Code))
                {
                    var airportDestination = await AirportAPIService.SearchByAirport(flightIn.AirportDestination.Code);

                    if (airportDestination == null)
                        return (flightIn, new ApiResponse(404, "Aeroporto de destino não encontrado."));

                    flightIn.AirportDestination = airportDestination;
                }

                await _flightMongoService.Update(id, flightIn);

                return (flightIn, new ApiResponse(204));
            }

            return (flight, new ApiResponse(404, "Vôo não encontrado."));
        }

        public async Task<(Flight, ApiResponse)> ValidateToRemove(string id)
        {
            var flight = await _flightMongoService.Get(id);

            if (flight == null)
                return (flight, new ApiResponse(404, "Vôo não encontrado."));

            await _flightMongoService.Remove(id);

            return (flight, new ApiResponse(200));
        }


    }
}
