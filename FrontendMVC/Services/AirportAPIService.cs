using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FrontendMVC.Models;
using Newtonsoft.Json;

namespace FrontendMVC.Services
{
    public class AirportAPIService
    {
        public static async Task<List<Airport>> ToListAsync()
        {
            HttpClient httpClient = new();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"https://localhost:44334/api/Airports");

                if (!response.IsSuccessStatusCode)
                    return null;

                var responseBody = await response.Content.ReadAsStringAsync();

                if (responseBody.ToString().Contains("erro"))
                    return null;

                var airports = JsonConvert.DeserializeObject<List<Airport>>(responseBody);

                return airports;
            }
            catch (HttpRequestException exception)
            {
                throw new HttpRequestException("Serviço indisponível", exception, System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
