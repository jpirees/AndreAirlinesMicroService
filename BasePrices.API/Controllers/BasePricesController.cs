using System.Collections.Generic;
using System.Threading.Tasks;
using BasePrices.API.Services;
using BasePrices.API.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Newtonsoft.Json;
using Utils.HttpApiResponse;
using Utils.Services;

namespace BasePrices.API.Controllers
{
    [EnableCors]
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
        [Authorize]
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
        [Authorize]
        public async Task<ActionResult<BasePrice>> Get(string id)
        {
            var basePrice = await _basePriceMongoService.Get(id);

            if (basePrice == null)
                return NotFound(new ApiResponse(404, "Preço base não encontrado."));

            return basePrice;
        }

        [HttpPost]
        [Authorize(Roles = "manager_baseprices")]
        public async Task<ActionResult<BasePrice>> Create(BasePrice basePrice)
        {
            var (_, response) = await _basePriceValidator.ValidateToCreate(basePrice);


            switch (response.StatusCode)
            {
                case 400:
                    return BadRequest(response);

                case 404:
                    return NotFound(response);

                default:
                    var objectAfterJson = JsonConvert.SerializeObject(basePrice).ToString();

                    await LogAPIService.RegisterLog(new Log(null, null, objectAfterJson, "post", "base_prices"));

                    return CreatedAtRoute("GetBasePrice", new { id = basePrice.Id }, basePrice);
            }
        }

        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "manager_baseprices")]
        public async Task<IActionResult> Update(string id, BasePrice basePrice)
        {
            var (basePriceOut, response) = await _basePriceValidator.ValidateToUpdate(id, basePrice);

            switch (response.StatusCode)
            {
                case 400:
                    return BadRequest(response);

                case 404:
                    return NotFound(response);

                default:
                    var objectBeforeJson = JsonConvert.SerializeObject(basePriceOut).ToString();

                    var objectAfterJson = JsonConvert.SerializeObject(basePrice).ToString();

                    await LogAPIService.RegisterLog(new Log(null, objectBeforeJson, objectAfterJson, "put", "base_prices"));

                    return NoContent();
            }

        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "manager_baseprices")]
        public async Task<IActionResult> Delete(string id)
        {
            var (basePriceOut, response) = await _basePriceValidator.ValidateToRemove(id);

            if (response.StatusCode.Equals(404))
                return NotFound(response);

            var objectBeforeJson = JsonConvert.SerializeObject(basePriceOut).ToString();

            await LogAPIService.RegisterLog(new Log(null, objectBeforeJson, null, "delete", "base_prices"));

            return NoContent();
        }
    }
}
