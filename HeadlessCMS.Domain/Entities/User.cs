namespace HeadlessCMS.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Password { get; set; }
        public string? RefreshTokens { get; set; }
    }
}