using System.Net.Http;
using System.Threading.Tasks;
using Models.Entities;
using Newtonsoft.Json;

namespace Utils.Services
{
    public class AirportAPIService
    {
        public static async Task<Airport> SearchByAirportId(int id)
        {
            HttpClient httpClient = new();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"https://localhost:44334/api/Airports/{id}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var responseBody = await response.Content.ReadAsStringAsync();

                if (responseBody.ToString().Contains("erro"))
                    return null;

                var airplane = JsonConvert.DeserializeObject<Airport>(responseBody);

                return airplane;
            }
            catch (HttpRequestException exception)
            {
                throw new HttpRequestException("Serviço indisponível", exception, System.Net.HttpStatusCode.InternalServerError);
            }
        }


        public static async Task<Airport> SearchByAirportCode(string code)
        {
            HttpClient httpClient = new();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"https://localhost:44334/api/Airports/Code/{code}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var responseBody = await response.Content.ReadAsStringAsync();

                if (responseBody.ToString().Contains("erro"))
                    return null;

                var airplane = JsonConvert.DeserializeObject<Airport>(responseBody);

                return airplane;
            }
            catch (HttpRequestException exception)
            {
                throw new HttpRequestException("Serviço indisponível", exception, System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
