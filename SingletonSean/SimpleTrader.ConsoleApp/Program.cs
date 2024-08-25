using Microsoft.AspNet.Identity;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
using SimpleTrader.Domain.Services.AuthenticationServices;
using SimpleTrader.EntityFramework;
using SimpleTrader.EntityFramework.Services;
namespace SimpleTrader.ConsoleApp
{
    internal class Program
    {
        static async void Main(string[] args)
        {
            IDataService<User> userService = new GenericDataService<User>(new SimpleTraderDbContextFactory());
            IAuthenticationService authenticationService = new AuthenticationService(new AccountDataService(new SimpleTraderDbContextFactory()), new PasswordHasher());
            //authenticationService.Register("tricdt1@gmail.com", "tricdt1", "123456", "123456");
            Console.WriteLine(userService.Create(new User() { Username = "tricdt", Email = "tricdt@gmrail.com", DatedJoined = new DateTime(2024, 08, 15), PasswordHash = "0123456", Id = 0 }));
            Console.ReadLine();
        }
    }
}
