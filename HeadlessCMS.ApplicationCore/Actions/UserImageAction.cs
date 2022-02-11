using AutoMapper;
using HeadlessCMS.ApplicationCore.Dtos;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence;

namespace HeadlessCMS.ApplicationCore.Resolvers
{
    public class UserImageAction : IMappingAction<User, UserDto>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserImageAction(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Process(User source, UserDto destination, ResolutionContext context)
        {
            if (source.ProfilePictureId != null)
            {
                Media media = _applicationDbContext.Set<Media>().FirstOrDefault(media => media.Id == source.ProfilePictureId);

                destination.ProfilePicture = media.Uri;
            }
        }
    }
}