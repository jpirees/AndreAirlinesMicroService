using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Newtonsoft.Json;
using Tickets.API.Services;
using Tickets.API.Validators;
using Utils.HttpApiResponse;
using Utils.Services;

namespace Tickets.API.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly TicketMongoService _ticketMongoService;
        private readonly TicketValidator _ticketValidator;

        public TicketsController(TicketMongoService ticketMongoService, TicketValidator ticketValidator)
        {
            _ticketMongoService = ticketMongoService;
            _ticketValidator = ticketValidator;
        }

        [HttpGet]
        [Authorize(Roles = "manager_tickets")]
        public async Task<ActionResult<List<Ticket>>> GetAll() =>
            await _ticketMongoService.Get();

        [HttpGet("{id:length(24)}", Name = "GetTicket")]
        [Authorize(Roles = "manager_tickets,clerk_tickets")]
        public async Task<ActionResult<Ticket>> Get(string id)
        {
            var ticket = await _ticketMongoService.Get(id);

            if (ticket == null)
                return NotFound(new ApiResponse(404, "Reserva não encontrada."));

            return ticket;
        }

        [HttpGet("Flight")]
        [Authorize(Roles = "manager_tickets,clerk_tickets")]
        public async Task<ActionResult<List<Ticket>>> GetByFlight(string id)
        {
            var tickets = await _ticketMongoService.GetByFlight(id);

            if (tickets == null)
                return NotFound(new ApiResponse(404, "Reserva(s) não encontrada(s)."));

            return tickets;
        }

        [HttpPost]
        [Authorize(Roles = "manager_tickets,clerk_tickets")]
        public async Task<ActionResult<Ticket>> Create(Ticket ticket)
        {
            var (_,  response) = await _ticketValidator.ValidateToCreate(ticket);

            switch (response.StatusCode)
            {
                case 400:
                    return BadRequest(response);

                case 404:
                    return NotFound(response);

                default:
                    var objectAfterJson = JsonConvert.SerializeObject(ticket).ToString();

                    await LogAPIService.RegisterLog(new Log(null, null, objectAfterJson, "post", "tickets"));

                    return CreatedAtRoute("GetTicket", new { id = ticket.Id }, ticket);
            }
        }

        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "manager_tickets,clerk_tickets")]
        public async Task<IActionResult> Update(string id, Ticket ticket)
        {
            var (ticketOut, response) = await _ticketValidator.ValidateToUpdate(id, ticket);

            switch (response.StatusCode)
            {
                case 400:
                    return BadRequest(response);

                case 404:
                    return NotFound(response);

                default:
                    var objectBeforeJson = JsonConvert.SerializeObject(ticketOut).ToString();

                    var objectAfterJson = JsonConvert.SerializeObject(ticket).ToString();

                    await LogAPIService.RegisterLog(new Log(null, objectBeforeJson, objectAfterJson, "put", "tickets"));

                    return NoContent();
            }
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "manager_tickets,clerk_tickets")]
        public async Task<IActionResult> Delete(string id)
        {
            var (ticketOut, response) = await _ticketValidator.ValidateToRemove(id);

            if (response.StatusCode.Equals(404))
                return NotFound(response);

            var objectBeforeJson = JsonConvert.SerializeObject(ticketOut).ToString();

            await LogAPIService.RegisterLog(new Log(null, objectBeforeJson, null, "delete", "tickets"));

            return NoContent();
        }
    }
}
