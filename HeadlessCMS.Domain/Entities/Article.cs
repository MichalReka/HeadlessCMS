namespace HeadlessCMS.Domain.Entities
{
    public class Article : BaseEntity
    {
        public string? Title { get; set; }
        public Guid LeadImageId { get; set; }
        public string? Content { get; set; }
        public ICollection<Media>? Medias { get; set; }
    }
}