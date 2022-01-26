using Azure.Storage.Blobs;
using HeadlessCMS.ApplicationCore.Core.Interfaces.Services;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence.Repositories;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;

namespace HeadlessCMS.ApplicationCore.Services
{
    public class MediaService : IMediaService
    {
        readonly IBaseEntityRepository _baseEntityRepository;
        readonly BlobContainerClient _blobContainerClient;
        readonly IHttpContextAccessor _httpContextAccessor;

        public MediaService(IBaseEntityRepository baseEntityRepository, BlobContainerClient blobContainerClient, IHttpContextAccessor httpContextAccessor)
        {
            _baseEntityRepository = baseEntityRepository;
            _blobContainerClient = blobContainerClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task PrepareArticleToStore(Article article)
        {
            await MapContentToMedia(article);

        }

        private async Task MapContentToMedia(Article article)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(article.Content);

            var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//img");

            ICollection<Media> medias = new List<Media>();
            foreach (var node in htmlNodes)
            {
                var src = node.GetAttributeValue("src", "");
                if (src != "")
                {
                    Guid guid = Guid.NewGuid();
                    var base64String = src.Split("base64,").Last();

                    var bytes = Convert.FromBase64String(base64String);

                    var extension = GetSubstringBetweenStrings(src, "data:image/", ";base64,");

                    BlobClient blobClient = _blobContainerClient.GetBlobClient(guid.ToString()+"."+extension);

                    try
                    {
                        await blobClient.UploadAsync(BinaryData.FromBytes(bytes));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Cannot upload");
                    }

                    node.SetAttributeValue("src", blobClient.Uri.ToString());

                    Media media = new Media{
                        Uri = blobClient.Uri.ToString()
                    };

                    medias.Add(media);
                    _baseEntityRepository.Add(media, _httpContextAccessor.HttpContext.User);
                }
            }

            StringWriter sw = new StringWriter();
            htmlDoc.Save(sw);
            
            article.Content = sw.ToString();
            article.Medias = medias;
            sw.Close();
        }

        private Guid CreateMediaFromLeadImage(byte[] leadImage)
        {
            return Guid.NewGuid();
        }

        private string GetSubstringBetweenStrings(string source, string firstSubstring, string secondSubstring)
        {
            int pFrom = source.IndexOf(firstSubstring) + firstSubstring.Length;
            int pTo = source.LastIndexOf(secondSubstring);

            return source.Substring(pFrom, pTo - pFrom);
        }
    }
}
