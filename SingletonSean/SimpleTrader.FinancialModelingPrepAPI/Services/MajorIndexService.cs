using Newtonsoft.Json;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
using System.Net.Http;
namespace SimpleTrader.FinancialModelingPrepAPI.Services
{
    public class MajorIndexService : IMajorIndexService
    {
        public async Task<MajorIndex> GetMajorIndex(MajorIndexType indexType)
        {
            using (HttpClient client = new HttpClient())
            {
                string _apikey = "UbaYiNzmGPpOt4JR3965DmMzL4664AlI";
                string uri = "https://financialmodelingprep.com/api/v3/quote-order/" + GetUriSuffix(indexType);

                HttpResponseMessage response = await client.GetAsync($"{uri}?apikey={_apikey}");
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Quote quoteResponse = JsonConvert.DeserializeObject<List<Quote>>(jsonResponse)[0];
                MajorIndex majorIndex = JsonConvert.DeserializeObject<List<MajorIndex>>(jsonResponse)[0];
                majorIndex.Type = indexType;
                majorIndex.Changes = Convert.ToDouble(quoteResponse.change);
                return majorIndex;
            }
        }
        private string GetUriSuffix(MajorIndexType indexType)
        {
            switch (indexType)
            {
                case MajorIndexType.DowJones:
                    return "AAPL";
                case MajorIndexType.Nasdaq:
                    return "IIXIX";
                case MajorIndexType.SP500:
                    return "IINX";
                default:
                    return "AAPL";
            }
        }
    }
}
