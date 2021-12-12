namespace HeadlessCMS.Domain.Entities
{
    public class Media : BaseEntity
    {
        public string Url { get; set; }
        public decimal Size { get; set; }
    }
}