using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KumportWeb.Models
{
    public class LoginViewModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Msg { get; set; }       
    }
}
