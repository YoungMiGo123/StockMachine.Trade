using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Core.Models.Http
{
    public class HttpManager
    {
        private readonly HttpClient client;

        public HttpManager()
        {
            client = new HttpClient();
        }

        public async Task<T> Get<T>(string url)
        {
            try
            {
                var responseString = await client.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<T>(responseString);
                return result;
            }
            catch
            {
                return default(T);
            }

        }
        public async Task<T> PostAsync<T>(string Url, string jsonString)
        {
            try
            {
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(responseString);
                return result;
            }
            catch
            {
                return default(T);
            }
        }
    }
}
