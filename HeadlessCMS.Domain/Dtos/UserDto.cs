using HeadlessCMS.Domain.Entities;

namespace HeadlessCMS.ApplicationCore.Dtos
{
    public class UserDto : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Media ProfilePicture { get; set; }
        public string Password { get; set; }
    }
}
