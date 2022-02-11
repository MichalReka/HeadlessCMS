using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using HeadlessCMS.ApplicationCore.Services;
using HeadlessCMS.ApplicationCore.Core.Interfaces.Services;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using HeadlessCMS.Domain.Interfaces;

namespace HeadlessCMS.ApplicationCore
{
    public static class DependencyInjection
    {
        public static void AddApplicationCoreServices(this IServiceCollection serviceCollection,
             IConfiguration configuration)
        {
            serviceCollection.AddScoped(_ => {
                var client = new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage"));
                var container = client.GetBlobContainerClient("images");
                container.CreateIfNotExists();
                container.SetAccessPolicy(PublicAccessType.BlobContainer);
                return container;
            });
            serviceCollection.AddScoped<IAuthService, AuthService>();
            serviceCollection.AddScoped<IArticleService, ArticleService>();
            serviceCollection.AddScoped<IMediaService, MediaService>();
            serviceCollection.AddScoped<IPermissionService, PermissionService>();
            serviceCollection.AddScoped<IUserService, UserService>();
        }
    }
}