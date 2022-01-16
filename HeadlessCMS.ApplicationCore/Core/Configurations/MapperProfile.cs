using AutoMapper;
using HeadlessCMS.ApplicationCore.Dtos;
using HeadlessCMS.Domain.Dtos;
using HeadlessCMS.Domain.Entities;

namespace HeadlessCMS.ApplicationCore.Core.Configurations
{
    public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<UserDto, User>();
			CreateMap<User, UserDto>();

            CreateMap<BaseEntity, BaseEntityDto>();
            CreateMap<BaseEntityDto, BaseEntity>();

            CreateMap<Article, ArticleDto>();
            CreateMap<ArticleDto, Article>();

            CreateMap<Media, MediaDto>();
            CreateMap<MediaDto, Media>();
        }

	}
}
