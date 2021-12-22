using AutoMapper;
using HeadlessCMS.ApplicationCore.Dtos;
using HeadlessCMS.Domain.Entities;

namespace HeadlessCMS.ApplicationCore.Core.Configurations
{
    public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<UserDto, User>();
			CreateMap<User, UserDto>();
		}
    
    }
}
