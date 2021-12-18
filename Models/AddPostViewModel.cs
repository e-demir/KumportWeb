using Microsoft.AspNetCore.Http;

namespace KumportWeb.Models
{
    public class AddPostViewModel
    {
        public IFormFile Image { get; set; }
        public string Title { get; set; }        
        public string Owner { get; set; }        
    }
}
