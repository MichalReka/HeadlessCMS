using HeadlessCMS.Persistence.Repositories;
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
            serviceCollection.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.UseLazyLoadingProxies();
            });
            serviceCollection.AddScoped<IBaseEntityRepository, BaseEntityRepository>();
        }

        public static void AddRepositories(this IServiceCollection serviceCollection)
        {
        }
    }
}