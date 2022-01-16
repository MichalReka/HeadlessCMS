

using HeadlessCMS.ApplicationCore.Dtos;
namespace HeadlessCMS.Domain.Dtos
{
    public class BaseEntityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid CreatedById { get; set; }
        public UserDto? CreatedBy { get; set; }
        public Guid UpdatedById { get; set; }
        public UserDto? UpdatedBy { get; set; }
    }
}
