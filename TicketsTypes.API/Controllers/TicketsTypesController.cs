using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Newtonsoft.Json;
using TicketsTypes.API.Services;
using TicketsTypes.API.Validators;
using Utils.HttpApiResponse;
using Utils.Services;

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
           var (_, response) = await _ticketTypeValidator.ValidateToCreate(ticketType);

            if (response.StatusCode.Equals(400))
                return BadRequest(response);

            var objectAfterJson = JsonConvert.SerializeObject(ticketType).ToString();

            await LogAPIService.RegisterLog(new Log(null, null, objectAfterJson, "post", "tickets_types"));

            return CreatedAtRoute("GetTicketType", new { id = ticketType.Id }, ticketType);
        }

        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "manager_ticketstypes")]
        public async Task<IActionResult> Update(string id, TicketType ticketType)
        {
           var (ticketTypeOut, response) = await _ticketTypeValidator.ValidateToUpdate(id, ticketType);

            switch (response.StatusCode)
            {
                case 400:
                    return BadRequest(response);

                case 404:
                    return NotFound(response);

                default:
                    var objectBeforeJson = JsonConvert.SerializeObject(ticketTypeOut).ToString();

                    var objectAfterJson = JsonConvert.SerializeObject(ticketType).ToString();

                    await LogAPIService.RegisterLog(new Log(null, objectBeforeJson, objectAfterJson, "put", "tickets_types"));

                    return NoContent();
            }
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "manager_ticketstypes")]
        public async Task<IActionResult> Delete(string id)
        {
            var (ticketTypeOut, response) = await _ticketTypeValidator.ValidateToRemove(id);

            if (response.StatusCode.Equals(404))
                return NotFound(response);

            var objectBeforeJson = JsonConvert.SerializeObject(ticketTypeOut).ToString();

            await LogAPIService.RegisterLog(new Log(null, objectBeforeJson, null, "delete", "tickets_types"));

            return NoContent();
        }
    }
}
