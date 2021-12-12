using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using HeadlessCMS.ApplicationCore.Services;
using HeadlessCMS.ApplicationCore.Core.Interfaces.Services;

namespace HeadlessCMS.ApplicationCore
{
    public static class DependencyInjection
    {
        public static void AddApplicationCoreServices(this IServiceCollection serviceCollection,
             IConfiguration configuration)
        {
            serviceCollection.AddScoped<ITokenService, TokenService>();
            serviceCollection.AddScoped<IUserAuthService, UserAuthService>();
        }
    }
}