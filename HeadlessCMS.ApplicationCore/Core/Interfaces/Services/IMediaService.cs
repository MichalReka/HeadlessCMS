using HeadlessCMS.Domain.Dtos;
using HeadlessCMS.Domain.Entities;

namespace HeadlessCMS.ApplicationCore.Core.Interfaces.Services
{
    public interface IMediaService
    {
        public Task PrepareArticleToStore(Article article);
    }
}