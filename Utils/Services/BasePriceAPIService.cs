using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;
using Newtonsoft.Json;

namespace Utils.Services
{
    public class BasePriceAPIService
    {
        public static async Task<BasePrice> SearchByFlightId(string id)
        {
            HttpClient httpClient = new();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"https://localhost:44383/api/BasePrices/{id}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var responseBody = await response.Content.ReadAsStringAsync();

                if (responseBody.ToString().Contains("erro"))
                    return null;

                var basePrice = JsonConvert.DeserializeObject<BasePrice>(responseBody);

                return basePrice;
            }
            catch (HttpRequestException exception)
            {
                throw new HttpRequestException("Serviço indisponível", exception, System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
