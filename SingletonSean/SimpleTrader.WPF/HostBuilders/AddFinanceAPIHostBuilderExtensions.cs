using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleTrader.FinancialModelingPrepAPI;
using SimpleTrader.FinancialModelingPrepAPI.Models;
namespace SimpleTrader.WPF.HostBuilders
{
    public static class AddFinanceAPIHostBuilderExtensions
    {
        public static IHostBuilder AddFinanceAPI(this IHostBuilder host)
        {
            host.ConfigureServices((context, services) =>
            {
                string _api_key = "UbaYiNzmGPpOt4JR3965DmMzL4664AlI";
                string baseUrl = "https://financialmodelingprep.com/api/v3/";
                services.AddSingleton(new FinancialModelingPrepAPIKey(_api_key));
                services.AddHttpClient<FinancialModelingPrepHttpClient>(c =>
                {
                    c.BaseAddress = new Uri(baseUrl);
                });
            });
            return host;
        }
    }
}
