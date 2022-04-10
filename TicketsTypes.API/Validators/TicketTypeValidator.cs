using System.Threading.Tasks;
using Models.Entities;
using TicketsTypes.API.Services;
using Utils.HttpApiResponse;

namespace TicketsTypes.API.Validators
{
    public class TicketTypeValidator
    {
        private readonly TicketTypeMongoService _ticketTypeMongoService;

        public TicketTypeValidator(TicketTypeMongoService ticketTypeMongoService)
        {
            _ticketTypeMongoService = ticketTypeMongoService;
        }

        public async Task<(TicketType, ApiResponse)> ValidateToCreate(TicketType ticketTypeIn)
        {
            var ticketType = await _ticketTypeMongoService.GetByDescription(ticketTypeIn.Description);

            if (ticketType != null)
                return (ticketType, new ApiResponse(400, "Classe já registrada."));

            await _ticketTypeMongoService.Create(ticketTypeIn);

            return (ticketType, new ApiResponse(201));
        }

        public async Task<(TicketType, ApiResponse)> ValidateToUpdate(string id, TicketType ticketTypeIn)
        {
            var ticketType = await _ticketTypeMongoService.Get(id);

            if (ticketType == null)
                return (ticketType, new ApiResponse(404, "Classe não encontrada."));

            if (ticketType.Description.Equals(ticketTypeIn.Description))
                return (ticketType, new ApiResponse(400, "Classe já registrada."));

            await _ticketTypeMongoService.Update(id, ticketTypeIn);

            return (ticketType, new ApiResponse(204));
        }

        public async Task<(TicketType, ApiResponse)> ValidateToRemove(string id)
        {
            var ticketType = await _ticketTypeMongoService.Get(id);

            if (ticketType == null)
                return (ticketType, new ApiResponse(404, "Classe não encontrada."));

            await _ticketTypeMongoService.Remove(id);

            return (ticketType, new ApiResponse(204));
        }
    }
}
