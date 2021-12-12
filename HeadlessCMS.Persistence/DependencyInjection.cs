using HeadlessCMS.ApplicationCore.Core.Interfaces.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HeadlessCMS.Persistence
{
    public static class DependencyInjection
    {
        public static void AddSQLDbContext(this IServiceCollection serviceCollection,
             IConfiguration configuration)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            serviceCollection.AddScoped<IDbSetsProvider, DbSetsProvider>();
        }

        public static void AddRepositories(this IServiceCollection serviceCollection)
        {
        }
    }
}