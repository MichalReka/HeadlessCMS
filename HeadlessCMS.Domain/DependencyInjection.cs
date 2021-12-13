using HeadlessCMS.Domain.Interfaces;
using HeadlessCMS.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HeadlessCMS.ApplicationCore
{
    public static class DependencyInjection
    {
        public static void AddDomainServices(this IServiceCollection serviceCollection,
             IConfiguration configuration)
        {
            serviceCollection.AddScoped<IPasswordEncryptService, PasswordEncryptService>();
            serviceCollection.AddScoped<ITokenService, TokenService>();
        }
    }
}