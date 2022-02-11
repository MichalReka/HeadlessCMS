using HeadlessCMS.Domain.Entities;

namespace HeadlessCMS.ApplicationCore.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? ProfilePicture { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
