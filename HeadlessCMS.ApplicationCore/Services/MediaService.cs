using Azure.Storage.Blobs;
using HeadlessCMS.ApplicationCore.Core.Interfaces.Services;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence;
using HeadlessCMS.Persistence.Repositories;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HeadlessCMS.ApplicationCore.Services
{
    public class MediaService : IMediaService
    {
        readonly IBaseEntityRepository _baseEntityRepository;
        readonly BlobContainerClient _blobContainerClient;
        readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _dbContext;

        public MediaService(IBaseEntityRepository baseEntityRepository, 
            BlobContainerClient blobContainerClient, 
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext dbContext)
        {
            _baseEntityRepository = baseEntityRepository;
            _blobContainerClient = blobContainerClient;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public async Task PrepareArticleToStore(Article article, string? leadImageString)
        {
            await MapContentToMedia(article);
            article.LeadImageId = leadImageString != null && leadImageString.Length > 0 ? (await CreateMediaFromImage(leadImageString)).Id : null;
        }

        public async Task PrepareUserToStore(User user, string? profileImageString)
        {
            user.ProfilePictureId = profileImageString != null ? (await CreateMediaFromImage(profileImageString)).Id : null;
        }

        private async Task MapContentToMedia(Article article)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(article.Content);

            var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//img");

            if(htmlNodes == null)
            {
                return;
            }

            ICollection<Media> medias = new List<Media>();
            foreach (var node in htmlNodes)
            {
                var src = node.GetAttributeValue("src", "");
                if (src != "")
                {
                    string uri = (await CreateMediaFromImage(src)).Uri;

                    node.SetAttributeValue("src", uri);
                }
            }

            StringWriter sw = new StringWriter();
            htmlDoc.Save(sw);
            
            article.Content = sw.ToString();
            article.Medias = medias;
            sw.Close();
        }

        private async Task<Media> CreateMediaFromImage(string imageString)
        {
            Media? image = GetMediaFromUri(imageString);

            if(image != null)
            {
                return image;
            }

            string uri = await UploadToBlob(imageString);

            image = CreateMediaFromUri(uri);

            return image;
        }

        private async Task<string> UploadToBlob(string img) {
            Guid guid = Guid.NewGuid();
            var base64String = img.Split("base64,").Last();

            var bytes = Convert.FromBase64String(base64String);

            var extension = GetSubstringBetweenStrings(img, "data:image/", ";base64,");

            BlobClient blobClient = _blobContainerClient.GetBlobClient(guid.ToString()+"."+extension);

            try
            {
                await blobClient.UploadAsync(BinaryData.FromBytes(bytes));
            }
            catch
            {
                throw new Exception("Cannot upload");
            }
            
            return blobClient.Uri.ToString();
        }

        private string GetSubstringBetweenStrings(string source, string firstSubstring, string secondSubstring)
        {
            int pFrom = source.IndexOf(firstSubstring) + firstSubstring.Length;
            int pTo = source.LastIndexOf(secondSubstring);

            return source.Substring(pFrom, pTo - pFrom);
        }

        private Media? GetMediaFromUri(string imageString)
        {
            if(imageString.StartsWith("http"))
            {
                var media = _dbContext.Set<Media>().AsNoTracking().FirstOrDefault(x => x.Uri == imageString);
                if(media != null)
                {
                    return media;
                }
                else
                {
                    return CreateMediaFromUri(imageString);
                }
            }

            return null;
        }

        private Media CreateMediaFromUri(string uri)
        {
            Media media = new Media
            {
                Uri = uri
            };

            var image = _baseEntityRepository.Add(media, _httpContextAccessor.HttpContext.User);
            return image;
        }
    }
}
