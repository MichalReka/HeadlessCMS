using AutoMapper;
using HeadlessCMS.ApplicationCore.Core.Interfaces.Services;
using HeadlessCMS.Domain.Dtos;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence.Repositories;
using Microsoft.AspNetCore.Http;

namespace HeadlessCMS.ApplicationCore.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IMediaService _mediaService;
        private readonly IMapper _mapper;
        private readonly IBaseEntityRepository _baseEntityRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ArticleService(IMediaService mediaService, IMapper mapper, IBaseEntityRepository baseEntityRepository, IHttpContextAccessor httpContextAccessor)
        {
            _mediaService = mediaService;
            _mapper = mapper;
            _baseEntityRepository = baseEntityRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Article> CreateArticle(ArticleDto articleDto)
        {
            ValidateArticle(articleDto);

            Article article = await PrepareArticle(articleDto);

            return _baseEntityRepository.Add(article, _httpContextAccessor.HttpContext.User);
        }

        public async Task<Article> UpdateArticle(ArticleDto articleDto)
        {
            ValidateArticle(articleDto);

            Article article = await PrepareArticle(articleDto);

            return _baseEntityRepository.Update(article, _httpContextAccessor.HttpContext.User);
        }

        private async Task<Article> PrepareArticle(ArticleDto articleDto)
        {
            Article article = _mapper.Map<Article>(articleDto);
            await _mediaService.PrepareArticleToStore(article, articleDto.LeadImage);
            return article;
        }

        private void ValidateArticle(ArticleDto articleDto)
        {
            if (articleDto.Content == null)
            {
                throw new ArgumentException("Artykuł musi posiadać treść");
            }

            if (articleDto.Title == null)
            {
                throw new ArgumentException("Artykuł musi posiadać treść");
            }
        }
    }
}