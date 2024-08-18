using SimpleTrader.Domain.Exceptions;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
namespace SimpleTrader.FinancialModelingPrepAPI.Services
{
    public class StockPriceService : IStockPriceService
    {
        private readonly FinancialModelingPrepHttpClient _client;

        public StockPriceService(FinancialModelingPrepHttpClient client)
        {
            _client = client;
        }

        public async Task<double> GetPrice(string symbol)
        {
            string uri = "quote-order/" + symbol;

            List<Quote> quotes = await _client.GetAsync<List<Quote>>(uri);
            Quote quote = quotes[0];
            if (Convert.ToDouble(quote.priceAvg50) == 0)
            {
                throw new InvalidSymbolException(symbol);
            }
            return Convert.ToDouble(quote.priceAvg50);
        }
    }
}
