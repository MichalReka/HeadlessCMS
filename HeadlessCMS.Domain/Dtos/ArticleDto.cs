using HeadlessCMS.Domain.Entities;

namespace HeadlessCMS.Domain.Dtos
{
    public class ArticleDto : BaseEntityDto
    {
        public string? Title { get; set; }
        public Byte[]? LeadImage { get; set; }
        public string? Content { get; set; }
        public ICollection<Media>? Medias { get; set; }
        public bool IsUnique { get; set; }
    }
}
