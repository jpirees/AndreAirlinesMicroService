using System.Collections.Generic;
using System.Threading.Tasks;
using Logs.API.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Newtonsoft.Json;
using Utils.HttpApiResponse;
using Utils.Services;

namespace Logs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly LogMongoService _logMongoService;

        public LogsController (LogMongoService logMongoService)
        {
            _logMongoService = logMongoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Log>>> Get() =>
            await _logMongoService.Get();

        [HttpGet("{id:length(24)}", Name = "GetLog")]
        public async Task<ActionResult<Log>> Get(string id)
        {
            var log = await _logMongoService.Get(id);

            if (log == null)
                return NotFound();

            return Ok(log);
        }

        [HttpPost]
        public async Task<ActionResult<Log>> Create(Log logIn)
        {
            //var user = await UserAPIService.SearchByUsername(logIn.User.ToString());

            //if (user == null)
            //    return NotFound(new ApiResponse(400, "Usuário não encontrado."));

            await _logMongoService.Create(logIn);

            return CreatedAtRoute("GetLog", new { id = logIn.Id.ToString() }, logIn);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var log = await _logMongoService.Get(id);

            if (log == null)
                return NotFound(new ApiResponse(400, "Log não encontrado."));

            await _logMongoService.Remove(id);

            return NoContent();
        }

    }
}
