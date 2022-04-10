using System.Net.Http;
using System.Threading.Tasks;
using Models.Entities;
using Newtonsoft.Json;

namespace Utils.Services
{
    public class AirplaneAPIService
    {
        public static async Task<Airplane> SearchByAirplane(string registrationCode)
        {
            HttpClient httpClient = new();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"https://localhost:44360/api/Airplanes/{registrationCode}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var responseBody = await response.Content.ReadAsStringAsync();

                if (responseBody.ToString().Contains("erro"))
                    return null;

                var airplane = JsonConvert.DeserializeObject<Airplane>(responseBody);

                return airplane;
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException("Serviço indisponível", e, System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
