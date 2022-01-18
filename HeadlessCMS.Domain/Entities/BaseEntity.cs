using HeadlessCMS.ApplicationCore.Dtos;

namespace HeadlessCMS.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid CreatedById { get; set; }
        public User? CreatedBy { get; set; }
        public Guid UpdatedById { get; set; }
        public User? UpdatedBy { get; set; }
    }
}