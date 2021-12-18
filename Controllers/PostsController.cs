using KumportWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace KumportWeb.Controllers
{
    public class PostsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var token = HttpContext.Session.GetString("token");
            if (!string.IsNullOrEmpty(token))
            {

                using (var client = new HttpClient())
                {
                    var url = "https://localhost:44305/api/post/posts";
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    var re = await client.GetAsync(url);
                    var z = re.Content.ReadAsAsync<List<PostModel>>().Result;
                    var indexModel = new IndexModel();
                    indexModel.Posts = new List<PostModel>();
                    indexModel.Posts = z;
                    indexModel.LoggedUserName = HttpContext.Session.GetString("username");
                    indexModel.LoggedIn = !string.IsNullOrEmpty(HttpContext.Session.GetString("username"));

                    return View(indexModel);
                }                
                
            }
            else
            {
                return RedirectToAction("Login", "Auth",new LoginViewModel() {Msg = "Log in to see the posts" });
            }


        }

        [HttpPost]
        public IActionResult Index(IFormFile files)
        {

            return View();
        }

        [HttpGet]
        public IActionResult AddPost()
        {
            var token = HttpContext.Session.GetString("token");
            if (!string.IsNullOrEmpty(token))
            {
                return View();
            }
            else
            {
                return View("Views/Auth/UnAuth.cshtml");
            }
        }

        [HttpPost]
        public IActionResult AddPost(AddPostViewModel model)
        {
            if (model.Image != null)
            {
                if (model.Image.Length > 0)
                {
                    //Getting FileName
                    var fileName = Path.GetFileName(model.Image.FileName);
                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);
                    // concatenating  FileName + FileExtension
                    var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

                    var post = new PostModel()
                    {
                        FileType = fileExtension,
                        CreatedOn = DateTime.Now,
                        PostTitle = model.Title,
                        PostOwner = HttpContext.Session.GetString("username")
                    };

                    using (var target = new MemoryStream())
                    {
                        model.Image.CopyTo(target);
                        post.Image = target.ToArray();
                    }

                    HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("post/add", post).Result;




                }
            }
            return RedirectToAction("Index");
        }
    }
}
