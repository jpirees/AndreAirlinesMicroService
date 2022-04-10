using System.Threading.Tasks;
using BasePrices.API.Services;
using Models.Entities;
using Utils.HttpApiResponse;
using Utils.Services;

namespace BasePrices.API.Validators
{
    public class BasePriceValidator
    {
        private readonly BasePriceMongoService _basePriceMongoService;

        public BasePriceValidator(BasePriceMongoService basePriceMongoService)
        {
            _basePriceMongoService = basePriceMongoService;
        }

        public async Task<(BasePrice, ApiResponse)> ValidateToCreate(BasePrice basePriceIn)
        {
            var airportOrigin = await AirportAPIService.SearchByAirportCode(basePriceIn.AirportOrigin.Code);

            if (airportOrigin == null)
                return (basePriceIn, new ApiResponse(404, "Aeroporto de origem não encontrado."));


            var airportDestination = await AirportAPIService.SearchByAirportCode(basePriceIn.AirportDestination.Code);

            if (airportDestination == null)
                return (basePriceIn, new ApiResponse(404, "Aeroporto de destino não encontrado."));


            var basePriceList = await _basePriceMongoService.GetByBasePrice(airportOrigin.Code, airportDestination.Code);

            if (basePriceList.Count != 0)
            {
                foreach (var basePrice in basePriceList)
                    if (basePrice.Price.Equals(basePriceIn.Price))
                        return (basePriceIn, new ApiResponse(400, "Preço base já cadastrado."));

            }

            basePriceIn.AirportOrigin = airportOrigin;
            basePriceIn.AirportDestination = airportDestination;

            await _basePriceMongoService.Create(basePriceIn);

            return (basePriceIn, new ApiResponse(201));
        }

        public async Task<(BasePrice, ApiResponse)> ValidateToUpdate(string id, BasePrice basePriceIn)
        {
            var basePrice = await _basePriceMongoService.Get(id);

            if (basePrice != null)
            {

                if (!basePrice.AirportOrigin.Code.Equals(basePriceIn.AirportOrigin.Code))
                {
                    var airportOrigin = await AirportAPIService.SearchByAirportId(basePriceIn.AirportOrigin.Id);

                    if (airportOrigin == null)
                        return (basePriceIn, new ApiResponse(404, "Aeroporto de origem não encontrado."));

                    basePriceIn.AirportOrigin = airportOrigin;
                }

                if (!basePrice.AirportDestination.Code.Equals(basePriceIn.AirportDestination.Code))
                {
                    var airportDestination = await AirportAPIService.SearchByAirportId(basePriceIn.AirportDestination.Id);

                    if (airportDestination == null)
                        return (basePriceIn, new ApiResponse(404, "Aeroporto de destino não encontrado."));

                    basePriceIn.AirportDestination = airportDestination;
                }

                await _basePriceMongoService.Update(id, basePriceIn);

                return (basePriceIn, new ApiResponse(204));
            }

            return (basePrice, new ApiResponse(404, "Preço base não encontrado."));
        }

        public async Task<(BasePrice, ApiResponse)> ValidateToRemove(string id)
        {
            var basePrice = await _basePriceMongoService.Get(id);

            if (basePrice == null)
                return (basePrice, new ApiResponse(404, "Preço base não encontrado."));

            await _basePriceMongoService.Remove(id);

            return (basePrice, new ApiResponse(200));
        }
    }
}
