using HeadlessCMS.ApplicationCore.Dtos;

namespace HeadlessCMS.Domain.Entities
{
    public class User : UserDto
    {
        public Guid Id { get; set; }
        public string? RefreshTokens { get; set; }
        public byte[]? Salt { get; set; }
        public List<User>? CreatedUsers { get; set; }
        public List<User>? UpdatedUsers { get; set; }
    }

    public enum UserRole
    {
        BaseUser,
        Admin
    }
}