using Newtonsoft.Json;
using SimpleTrader.FinancialModelingPrepAPI.Models;
using System.Net.Http;

namespace SimpleTrader.FinancialModelingPrepAPI
{
    public class FinancialModelingPrepHttpClient
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        public FinancialModelingPrepHttpClient(HttpClient client, FinancialModelingPrepAPIKey apikey)
        {
            _client = client;
            _apiKey = apikey.Key;
        }
        public async Task<T> GetAsync<T>(string uri)
        {
            HttpResponseMessage response = await _client.GetAsync($"{uri}?apikey={_apiKey}");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }
    }
}
