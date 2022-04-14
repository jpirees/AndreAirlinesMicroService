using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;
using Models.Entities.DTO;
using Newtonsoft.Json;

namespace Utils.Services
{
    public class UserAPIService
    {
        public static async Task<UserResponseDTO> SearchByUsername(string username)
        {
            HttpClient httpClient = new();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"https://localhost:44334/api/Users/{username}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var responseBody = await response.Content.ReadAsStringAsync();

                if (responseBody.ToString().Contains("erro"))
                    return null;

                var user = JsonConvert.DeserializeObject<UserResponseDTO>(responseBody);

                return user;
            }
            catch (HttpRequestException exception)
            {
                throw new HttpRequestException("Serviço indisponível", exception, System.Net.HttpStatusCode.InternalServerError);
            }
        }


        public static async Task<UserResponseDTO> SearchByUsername(string username, string password)
        {
            HttpClient httpClient = new();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"https://localhost:44334/api/Users/{username}?password={password}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var responseBody = await response.Content.ReadAsStringAsync();

                if (responseBody.ToString().Contains("erro"))
                    return null;

                var user = JsonConvert.DeserializeObject<UserResponseDTO>(responseBody);

                return user;
            }
            catch (HttpRequestException exception)
            {
                throw new HttpRequestException("Serviço indisponível", exception, System.Net.HttpStatusCode.InternalServerError);
            }
        }


    }
}
