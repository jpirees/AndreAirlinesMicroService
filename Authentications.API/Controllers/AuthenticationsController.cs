using System;
using System.Threading.Tasks;
using Authentications.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.Entities.DTO;

namespace Authentications.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] UserRequestDTO userIn)
        {
            (var user, var reponse) = await AuthenticationService.GetUserAsync(userIn);

            if (reponse.StatusCode != 200)
                return NotFound(reponse);

            var token = TokenService.GenerateToken(user);

            return new { user, token };
        }

    }
}
