using System.Threading.Tasks;
using Models.Entities;
using Tickets.API.Services;
using Utils.HttpApiResponse;
using Utils.Services;

namespace Tickets.API.Validators
{
    public class TicketValidator
    {
        private readonly TicketMongoService _ticketMongoService;

        public TicketValidator(TicketMongoService ticketMongoService)
        {
            _ticketMongoService = ticketMongoService;
        }

        public async Task<(Ticket, ApiResponse)> ValidateToCreate(Ticket ticketIn)
        {
            var ticket = await _ticketMongoService.GetByPassengerOnFlight(ticketIn.Passenger.PassaportNumber, ticketIn.Flight.Id);

            if (ticket != null)
                return (ticketIn, new ApiResponse(400, "Passageiro já possui reserva nesse vôo."));

            var passenger = await PassengerAPIService.SearchByPassengerPassaportNumber(ticketIn.Passenger.PassaportNumber);

            if (passenger == null)
                return (ticketIn, new ApiResponse(404, "Passageiro não encontrado."));


            var flight = await FlightAPIService.SearchByFlightId(ticketIn.Flight.Id);

            if (flight == null)
                return (ticketIn, new ApiResponse(404, "Vôo não encontrado."));


            var basePrice = await BasePriceAPIService.SearchByFlightId(ticketIn.BasePrice.Id);

            if (basePrice == null)
                return (ticketIn, new ApiResponse(404, "Preço base não registrado."));


            var ticketType = await TicketTypeAPIService.SearchByTicketTypeId(ticketIn.TicketType.Id);

            if (ticketType == null)
                return (ticketIn, new ApiResponse(404, "Classe indisponível."));


            ticketIn.Passenger = passenger;
            ticketIn.Flight = flight;
            ticketIn.BasePrice = basePrice;
            ticketIn.TicketType = ticketType;

            ticketIn.TotalPrice = (basePrice.Price + ticketType.Price) * (1 - (ticketIn.DiscountPercentage / 100));

            await _ticketMongoService.Create(ticketIn);

            return (ticketIn, new ApiResponse(201));
        }

        public async Task<(Ticket, ApiResponse)> ValidateToUpdate(string id, Ticket ticketIn)
        {
            var ticket = await _ticketMongoService.Get(id);

            if (ticket != null)
            {
                if (!ticket.Passenger.PassaportNumber.Equals(ticketIn.Passenger.PassaportNumber))
                {
                    var passenger = await PassengerAPIService.SearchByPassengerPassaportNumber(ticketIn.Passenger.PassaportNumber);

                    if (passenger == null)
                        return (ticketIn, new ApiResponse(404, "Passageiro não encontrado."));

                    ticketIn.Passenger = passenger;
                }

                if (!ticket.Flight.Id.Equals(ticketIn.Flight.Id))
                {
                    var flight = await FlightAPIService.SearchByFlightId(ticketIn.Flight.Id);

                    if (flight == null)
                        return (ticketIn, new ApiResponse(404, "Vôo não encontrado."));

                    ticketIn.Flight = flight;
                }

                if (!ticket.BasePrice.Price.Equals(ticketIn.BasePrice.Price))
                {
                    var basePrice = await BasePriceAPIService.SearchByFlightId(ticketIn.BasePrice.Id);

                    if (basePrice == null)
                        return (ticketIn, new ApiResponse(404, "Preço base não registrado."));

                    ticketIn.BasePrice = basePrice;
                }

                if (!ticket.TicketType.Id.Equals(ticketIn.TicketType.Id))
                {
                    var ticketType = await TicketTypeAPIService.SearchByTicketTypeId(ticketIn.TicketType.Id);

                    if (ticketType == null)
                        return (ticketIn, new ApiResponse(404, "Classe indisponível."));

                    ticketIn.TicketType = ticketType;
                }

                ticketIn.TotalPrice = (ticketIn.BasePrice.Price + ticketIn.TicketType.Price) * (1 - (ticketIn.DiscountPercentage / 100));

                await _ticketMongoService.Update(id, ticketIn);

                return (ticketIn, new ApiResponse(204));
            }

            return (ticket, new ApiResponse(404, "Reserva não encontrada."));
        }

        public async Task<(Ticket, ApiResponse)> ValidateToRemove(string id)
        {
            var ticket = await _ticketMongoService.Get(id);

            if (ticket == null)
                return (ticket, new ApiResponse(404, "Reserva não encontrada."));

            await _ticketMongoService.Remove(id);

            return (ticket, new ApiResponse(200));
        }

    }
}
