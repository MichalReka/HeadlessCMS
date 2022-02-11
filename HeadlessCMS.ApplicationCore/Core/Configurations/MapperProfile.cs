using AutoMapper;
using HeadlessCMS.ApplicationCore.Dtos;
using HeadlessCMS.ApplicationCore.Resolvers;
using HeadlessCMS.Domain.Dtos;
using HeadlessCMS.Domain.Entities;

namespace HeadlessCMS.ApplicationCore.Core.Configurations
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>().AfterMap<UserImageAction>();

            CreateMap<BaseEntity, BaseEntityDto>();
            CreateMap<BaseEntityDto, BaseEntity>();

            CreateMap<ArticleDto, Article>();
            CreateMap<Article, ArticleDto>().AfterMap<ArticleImageAction>();

            CreateMap<Media, MediaDto>();
            CreateMap<MediaDto, Media>();
        }
    }
}