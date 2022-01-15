using HeadlessCMS.Domain.Entities;

namespace HeadlessCMS.ApplicationCore.Dtos
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public Media? ProfilePicture { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public List<Media>? CreatedMedias { get; set; }
        public List<Media>? UpdatedMedias { get; set; }
        public List<Article>? CreatedArticles { get; set; }        
        public List<Article>? UpdatedArticles { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
