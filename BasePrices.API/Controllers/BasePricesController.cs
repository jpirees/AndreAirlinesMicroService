using System.Collections.Generic;
using System.Threading.Tasks;
using BasePrices.API.Services;
using BasePrices.API.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Utils.HttpApiResponse;


namespace BasePrices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasePricesController : ControllerBase
    {
        private readonly BasePriceMongoService _basePriceMongoService;
        private readonly BasePriceValidator _basePriceValidator;

        public BasePricesController(BasePriceMongoService basePriceMongoService, BasePriceValidator basePriceValidator)
        {
            _basePriceMongoService = basePriceMongoService;
            _basePriceValidator = basePriceValidator;
        }

        [HttpGet]
        //public async Task<ActionResult<List<BasePrice>>> GetAll() =>
        //   await _basePriceMongoService.Get();

        public async Task<ActionResult<List<BasePrice>>> GetBasePrices(string? from = null, string? to = null)
        {
            if (string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
                return await _basePriceMongoService.Get();

            else if (!string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
                return await _basePriceMongoService.GetByAirportOrigin(from);

            else if (string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
                return await _basePriceMongoService.GetByAirportDestination(to);

            else
                return await _basePriceMongoService.GetByBasePrice(from, to);
        }


        [HttpGet("{id:length(24)}", Name = "GetBasePrice")]
        public async Task<ActionResult<BasePrice>> Get(string id)
        {
            var basePrice = await _basePriceMongoService.Get(id);

            if (basePrice == null)
                return NotFound(new ApiResponse(404, "Preço base não encontrado."));

            return basePrice;
        }

        [HttpPost]
        public async Task<ActionResult<BasePrice>> Create(BasePrice basePrice)
        {
            (_, var response) = await _basePriceValidator.ValidateToCreate(basePrice);

            return response.StatusCode switch
            {
                400 => BadRequest(response),
                404 => NotFound(response),
                _ => CreatedAtRoute("GetBasePrice", new { id = basePrice.Id }, basePrice)
            };
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, BasePrice basePrice)
        {
            (_, var response) = await _basePriceValidator.ValidateToUpdate(id, basePrice);

            return response.StatusCode switch
            {
                400 => BadRequest(response),
                404 => NotFound(response),
                _ => NoContent()
            };
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            (_, var response) = await _basePriceValidator.ValidateToRemove(id);

            if (response.StatusCode.Equals(404))
                return NotFound(response);

            return NoContent();
        }
    }
}
