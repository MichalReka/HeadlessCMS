namespace HeadlessCMS.Domain.Dtos
{
    public class ArticleDto : BaseEntityDto
    {
        public string? Title { get; set; }
        public byte[]? LeadImage { get; set; }
        public string? Content { get; set; }
    }
}