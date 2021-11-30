using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCMS.Domain.Entities
{
    public class Article : BaseEntity
    { 
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public ICollection<Media>? Medias { get; set;}
        public bool IsUnique { get; set; }
    }
}
