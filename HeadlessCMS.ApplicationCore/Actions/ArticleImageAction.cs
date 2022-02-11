using AutoMapper;
using HeadlessCMS.Domain.Dtos;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence;

namespace HeadlessCMS.ApplicationCore.Resolvers
{
    public class ArticleImageAction : IMappingAction<Article, ArticleDto>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ArticleImageAction(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Process(Article source, ArticleDto destination, ResolutionContext context)
        {
            if (source.LeadImageId != null)
            {
                Media media = _applicationDbContext.Set<Media>().FirstOrDefault(media => media.Id == source.LeadImageId);

                destination.LeadImage = media.Uri;
            }
        }
    }
}