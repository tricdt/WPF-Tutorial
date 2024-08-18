using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
namespace SimpleTrader.FinancialModelingPrepAPI.Services
{
    public class MajorIndexService : IMajorIndexService
    {
        private readonly FinancialModelingPrepHttpClient _client;

        public MajorIndexService(FinancialModelingPrepHttpClient client)
        {
            _client = client;
        }

        public async Task<MajorIndex> GetMajorIndex(MajorIndexType indexType)
        {
            string uri = "quote-order/" + GetUriSuffix(indexType);

            List<Quote> quotes = await _client.GetAsync<List<Quote>>(uri);
            Quote quote = quotes[0];
            List<MajorIndex> majorIndexs = await _client.GetAsync<List<MajorIndex>>(uri);
            MajorIndex majorIndex = majorIndexs[0];
            majorIndex.Type = indexType;
            majorIndex.IndexName = quote.symbol;
            majorIndex.Changes = Convert.ToDouble(quote.change);
            return majorIndex;
        }
        private string GetUriSuffix(MajorIndexType indexType)
        {
            switch (indexType)
            {
                case MajorIndexType.DowJones:
                    return "AAPL";
                case MajorIndexType.Nasdaq:
                    return "HIINX";
                case MajorIndexType.SP500:
                    return "AACI";
                default:
                    return "AAPL";
            }
        }
    }
}
