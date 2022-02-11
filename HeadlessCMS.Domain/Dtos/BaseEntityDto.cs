using HeadlessCMS.ApplicationCore.Dtos;
namespace HeadlessCMS.Domain.Dtos
{
    public class BaseEntityDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid CreatedById { get; set; }
        public virtual UserDto? CreatedBy { get; set; }
        public Guid UpdatedById { get; set; }
        public virtual UserDto? UpdatedBy { get; set; }
        public Guid OwnerId { get; set; }
        public virtual UserDto? Owner { get; set; }
    }
}
