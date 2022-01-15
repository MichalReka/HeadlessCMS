using HeadlessCMS.ApplicationCore.Dtos;

namespace HeadlessCMS.Domain.Entities
{
    public class User : UserDto
    {
        public string? RefreshTokens { get; set; }
        public byte[]? Salt { get; set; }
    }

    public enum UserRole
    {
        BaseUser,
        Admin
    }
}