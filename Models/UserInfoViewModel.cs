using System.Collections.Generic;

namespace KumportWeb.Models
{
    public class UserInfoViewModel
    {
        public List<PostModel> Posts { get; set; }        
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
