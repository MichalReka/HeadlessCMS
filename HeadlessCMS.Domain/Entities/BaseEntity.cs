using HeadlessCMS.ApplicationCore.Dtos;

namespace HeadlessCMS.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public Guid UpdatedById { get; set; }
        public virtual User UpdatedBy { get; set; }
        public Guid OwnerId { get; set; }
        public virtual User Owner { get; set; }
    }
}