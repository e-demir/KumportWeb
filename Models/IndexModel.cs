using System.Collections.Generic;

namespace KumportWeb.Models
{
    public class IndexModel
    {
        public List<PostModel> Posts { get; set; }

        public bool LoggedIn { get; set; }
        public string LoggedUserName { get; set; }

    }
}
