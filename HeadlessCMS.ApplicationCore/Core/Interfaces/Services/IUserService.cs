using HeadlessCMS.ApplicationCore.Dtos;
using HeadlessCMS.Domain.Commands;
using HeadlessCMS.Domain.Entities;
using System.Security.Claims;

namespace HeadlessCMS.ApplicationCore.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> UpdateUserDataAsync(UserDto userDto);
        void UpdateUserPassword(ChangePasswordCommand changePasswordCommand);
    }
}