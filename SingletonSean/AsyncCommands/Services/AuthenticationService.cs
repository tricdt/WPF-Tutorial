namespace AsyncCommands.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public async Task Login(string username)
        {
            await Task.Delay(1000);
        }
    }
}
