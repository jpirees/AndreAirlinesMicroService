using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using TicketsTypes.API.Services;
using TicketsTypes.API.Validators;
using Utils.HttpApiResponse;

namespace TicketsTypes.API.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsTypesController : ControllerBase
    {
        private readonly TicketTypeMongoService _ticketTypeMongoService;
        private readonly TicketTypeValidator _ticketTypeValidator;

        public TicketsTypesController(TicketTypeMongoService ticketTypeMongoService, TicketTypeValidator ticketTypeValidator)
        {
            _ticketTypeMongoService = ticketTypeMongoService;
            _ticketTypeValidator = ticketTypeValidator;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<TicketType>>> GetAll() =>
           await _ticketTypeMongoService.Get();


        [HttpGet("{id:length(24)}", Name = "GetTicketType")]
        [Authorize]
        public async Task<ActionResult<TicketType>> Get(string id)
        {
            var ticketType = await _ticketTypeMongoService.Get(id);

            if (ticketType == null)
                return NotFound(new ApiResponse(404, "Classe não encontrada."));

            return ticketType;
        }


        [HttpGet("{description}")]
        [Authorize]
        public async Task<ActionResult<TicketType>> GetByDescription(string description)
        {
            var ticketType = await _ticketTypeMongoService.GetByDescription(description);

            if (ticketType == null)
                return NotFound(new ApiResponse(404, "Classe não encontrada."));

            return ticketType;
        }

        [HttpPost]
        [Authorize(Roles = "manager_ticketstypes")]
        public async Task<ActionResult<TicketType>> Create(TicketType ticketType)
        {
            (_, var response) = await _ticketTypeValidator.ValidateToCreate(ticketType);

            if (response.StatusCode.Equals(400))
                return BadRequest(response);
            
            return CreatedAtRoute("GetTicketType", new { id = ticketType.Id }, ticketType);
        }

        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "manager_ticketstypes")]
        public async Task<IActionResult> Update(string id, TicketType ticketType)
        {
            (_, var response) = await _ticketTypeValidator.ValidateToUpdate(id, ticketType);

            return response.StatusCode switch
            {
                400 => BadRequest(response),
                404 => NotFound(response),
                _ => NoContent()
            };
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "manager_ticketstypes")]
        public async Task<IActionResult> Delete(string id)
        {
            (_, var response) = await _ticketTypeValidator.ValidateToRemove(id);


            if (response.StatusCode.Equals(404))
                return NotFound(response);

            return NoContent();
        }
    }
}
