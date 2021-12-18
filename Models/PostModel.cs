using System;

namespace KumportWeb.Models
{
    public class PostModel
    {
        
        public int PostId { get; set; }
        
        public string PostTitle { get; set; }

        public string PostOwner { get; set; }
        
        public string FileType { get; set; }
        
        public byte[] Image { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}
