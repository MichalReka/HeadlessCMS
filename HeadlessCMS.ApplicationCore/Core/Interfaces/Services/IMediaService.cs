using HeadlessCMS.Domain.Entities;

namespace HeadlessCMS.ApplicationCore.Core.Interfaces.Services
{
    public interface IMediaService
    {
        public Task PrepareArticleToStore(Article article, string? leadImageString);
        public Task PrepareUserToStore(User user, string? profileImageString);
    }
}