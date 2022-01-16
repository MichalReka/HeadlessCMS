namespace HeadlessCMS.Domain.Dtos
{
    public class MediaDto : BaseEntityDto
    {
        public byte[] Content { get; set; }
        public decimal Size { get; set; }
    }
}
