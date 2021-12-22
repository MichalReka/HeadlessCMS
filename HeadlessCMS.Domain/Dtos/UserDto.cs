using HeadlessCMS.Domain.Entities;

namespace HeadlessCMS.ApplicationCore.Dtos
{
    public class UserDto : BaseEntity
    {
        public string Password { get; set; }
    }
}
