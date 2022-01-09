namespace HeadlessCMS.Domain.Entities
{
    public class Media : BaseEntity
    {
        public byte[] Content { get; set; }
        public decimal Size { get; set; }
    }
}