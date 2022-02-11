using HeadlessCMS.ApplicationCore.Core.Interfaces.Services;
using HeadlessCMS.ApplicationCore.Dtos;
using HeadlessCMS.Domain.Commands;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Domain.Interfaces;
using HeadlessCMS.Persistence;
using System.Security.Claims;

namespace HeadlessCMS.ApplicationCore.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordEncryptService _passwordEncryptService;
        private readonly IMediaService _mediaService;
        private readonly ApplicationDbContext _aplicationDbContext;

        public UserService(IPasswordEncryptService passwordEncryptService, ApplicationDbContext aplicationDbContext, IMediaService mediaService)
        {
            _passwordEncryptService=passwordEncryptService;
            _aplicationDbContext=aplicationDbContext;
            _mediaService=mediaService;
        }

        public async Task<User> UpdateUserDataAsync(UserDto userDto)
        {
            var userSet = _aplicationDbContext.Set<User>();

            var user = userSet.FirstOrDefault(x => x.Id == userDto.Id);
            user.Role = userDto.Role;
            user.Name = userDto.Name;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            await _mediaService.PrepareUserToStore(user, userDto.ProfilePicture);

            userSet.Update(user);

            return user;
        }

        public void UpdateUserPassword(ChangePasswordCommand changePasswordCommand)
        {
            var userSet = _aplicationDbContext.Set<User>();

            var user = userSet.FirstOrDefault(x => x.Id == changePasswordCommand.UserId);

            bool validPassword = _passwordEncryptService.VerifyHashedPassword(user.Password, user.Salt, changePasswordCommand.OldPassword);

            if (!validPassword)
            {
                throw new Exception("Wprowadzono złe hasło!");
            }

            (user.Password, user.Salt) = _passwordEncryptService.HashPassword(changePasswordCommand.NewPassword);

            userSet.Update(user);


        }
    }
}
