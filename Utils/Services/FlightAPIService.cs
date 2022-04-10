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
    public class FlightAPIService
    {
        public static async Task<Flight> SearchByFlightId(string id)
        {
            HttpClient httpClient = new();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"https://localhost:44307/api/Flights/{id}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var responseBody = await response.Content.ReadAsStringAsync();

                if (responseBody.ToString().Contains("erro"))
                    return null;

                var flight = JsonConvert.DeserializeObject<Flight>(responseBody);

                return flight;
            }
            catch (HttpRequestException exception)
            {
                throw new HttpRequestException("Serviço indisponível", exception, System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
