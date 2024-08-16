using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
using SimpleTrader.EntityFramework;
using SimpleTrader.EntityFramework.Services;
namespace SimpleTrader.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IDataService<User> userService = new GenericDataService<User>(new SimpleTraderDbContextFactory());
            Console.WriteLine(userService.Create(new User() { Username = "tricdt", Email = "tricdt@gmrail.com", DatedJoined = new DateTime(2024, 08, 15), Password = "0123456", Id = 0 }));

            Console.ReadLine();
        }
    }
}
