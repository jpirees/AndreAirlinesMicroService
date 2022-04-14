using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;
using Newtonsoft.Json;

namespace Utils.Services
{
    public class LogAPIService
    {

        public static async Task RegisterLog(Log log)
        {
            HttpClient client = new();

            try
            {
                if (client.BaseAddress == null) client.BaseAddress = new Uri("https://localhost:44361/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                await client.PostAsJsonAsync("api/LogsProducer", log);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        public static async Task Send(Log log)
        {
            HttpClient client = new();

            try
            {
                if (client.BaseAddress == null) client.BaseAddress = new Uri("https://localhost:44312/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsJsonAsync("api/Logs", log);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }



    }
}
