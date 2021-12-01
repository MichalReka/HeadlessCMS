using HeadlessCMS.Domain.Interfaces.Repositories;
using HeadlessCMS.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HeadlessCMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IArticleRepository, ArticleRepository>();
        }
    }
}