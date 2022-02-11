using HeadlessCMS.ApplicationCore.Dtos;

namespace HeadlessCMS.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? RefreshTokens { get; set; }
        public byte[]? Salt { get; set; }
        public Guid? ProfilePictureId { get; set; }
        public virtual ICollection<User>? CreatedUsers { get; set; }
        public virtual ICollection<User>? UpdatedUsers { get; set; }
        public virtual ICollection<Media>? CreatedMedias { get; set; }
        public virtual ICollection<Media>? UpdatedMedias { get; set; }
        public virtual ICollection<Article>? CreatedArticles { get; set; }
        public virtual ICollection<Article>? UpdatedArticles { get; set; }
    }

    public enum UserRole
    {
        Editor,
        Admin
    }
}