namespace HeadlessCMS.Domain.Dtos
{
    public class ArticleDto : BaseEntityDto
    {
        public string Title { get; set; }
        public string? LeadImage { get; set; }
        public string Content { get; set; }
    }
}