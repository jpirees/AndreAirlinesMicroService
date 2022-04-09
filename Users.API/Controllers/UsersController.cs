using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Users.API.Services;
using Users.API.Validators;
using Utils.HttpApiResponse;

namespace Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserMongoService _userMongoService;
        private readonly UserValidator _userValidator;

        public UsersController(UserMongoService userMongoService, UserValidator userValidator)
        {
            _userMongoService = userMongoService;
            _userValidator = userValidator;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll() =>
           await _userMongoService.Get();


        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var user = await _userMongoService.Get(id);

            if (user == null)
                return NotFound(new ApiResponse(404, "Usuário não encontrado."));

            return user;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<User>> GetByUsername(string username)
        {
            var user = await _userMongoService.GetByUsername(username);

            if (user == null)
                return NotFound(new ApiResponse(404, "Usuário não encontrado."));

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create(User user)
        {
            (_, var response) = await _userValidator.ValidateToCreate(user);

            if (response.StatusCode.Equals(400))
                return BadRequest(response);

            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, User user)
        {
            (_, var response) = await _userValidator.ValidateToUpdate(id, user);

            if (response.StatusCode.Equals(404))
                return NotFound(response);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            (_, var response) = await _userValidator.ValidateToRemove(id);

            if (response.StatusCode.Equals(404))
                return NotFound(response);

            return NoContent();
        }
    }
}
