using Kumport.Common.RequestModels;
using Kumport.Common.ResponseModels;
using KumportWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace KumportWeb.Controllers
{
    public class AuthController : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var request = new LoginRequestModel();

            request.Email = model.Email;
            request.Password = model.Password;

            var response = GlobalVariables.WebApiClient.PostAsJsonAsync("Auth/login", request).Result;
            var content = response.Content.ReadAsStringAsync();
            var contentModel = JsonConvert.DeserializeObject<LoginResponseModel>(content.Result);

            if (string.IsNullOrEmpty(contentModel.ReturnMessage))
            {
                HttpContext.Session.SetString("token", contentModel.Token);
                HttpContext.Session.SetString("username", contentModel.Username);
                ViewBag.Username = "Value";

                return RedirectToAction("Index", "Posts");
            }
            else
            {
                model.Msg = contentModel.ReturnMessage;
                return View(model);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("token");
            return RedirectToAction("Index", "Posts");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            var request = new RegisterRequestModel();
            if (!string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.Username))
            {
                request.Email = model.Email;
                request.Password = model.Password;
                request.Username = model.Username;

                var response = GlobalVariables.WebApiClient.PostAsJsonAsync("Auth/register", request).Result;
                var content = response.Content.ReadAsStringAsync();
                var contentModel = JsonConvert.DeserializeObject<RegisterResponseModel>(content.Result);

                if (contentModel.IsSuccesful)
                {
                    model.Message = "";
                    return RedirectToAction("login", "auth");
                }
                else
                {
                    model.Message = contentModel.ReturnMessage;
                    return View(model);
                }
            }
            else
            {
                return View();
            }
        }     
    }
}
