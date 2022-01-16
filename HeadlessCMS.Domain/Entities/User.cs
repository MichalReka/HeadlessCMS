using HeadlessCMS.ApplicationCore.Dtos;

namespace HeadlessCMS.Domain.Entities
{
    public class User : UserDto
    {
        public string? RefreshTokens { get; set; }
        public byte[]? Salt { get; set; }
        public List<User>? CreatedUsers { get; set; }
        public List<User>? UpdatedUsers { get; set; }
        public List<Media>? CreatedMedias { get; set; }
        public List<Media>? UpdatedMedias { get; set; }
        public List<Article>? CreatedArticles { get; set; }
        public List<Article>? UpdatedArticles { get; set; }
    }

    public enum UserRole
    {
        BaseUser,
        Admin
    }
}