using KumportWeb.Models;
using Kumport.Common.RequestModels;
using Kumport.Common.ResponseModels;
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
            var indexModel = new IndexModel();
            var token = HttpContext.Session.GetString("token");
            if (!string.IsNullOrEmpty(token))
            {

                using (var client = new HttpClient())
                {
                    var url = "https://localhost:44305/api/post/posts";
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    var response = await client.GetAsync(url);
                    try
                    {
                        var responseAsModel = response.Content.ReadAsAsync<PostsResponseModel>().Result;
                        indexModel.Posts = new List<PostModel>();
                        foreach (var item in responseAsModel.Posts)
                        {
                            var model = new PostModel()
                            {
                                CreatedOn = item.CreatedOn,
                                FileType = item.FileType,
                                Image = item.Image,
                                PostOwner = item.PostOwner,
                                PostTitle = item.PostTitle
                            };
                            indexModel.Posts.Add(model);
                        }
                        //indexModel.Posts = responseAsModel;
                        indexModel.LoggedUserName = HttpContext.Session.GetString("username");
                        indexModel.LoggedIn = !string.IsNullOrEmpty(HttpContext.Session.GetString("username"));
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }

                    return View(indexModel);
                }

            }
            else
            {
                return RedirectToAction("Login", "Auth");
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
        public async Task<IActionResult> AddPost(AddPostViewModel model)
        {
            var requestModel = new AddPostRequestModel();
            var token = HttpContext.Session.GetString("token");
            if (model.Image != null)
            {
                if (model.Image.Length > 0)
                {
                    var fileName = Path.GetFileName(model.Image.FileName);
                    var fileExtension = Path.GetExtension(fileName);
                    var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);
                    requestModel.FileType = fileExtension;
                    requestModel.CreatedOn = DateTime.Now;
                    requestModel.PostTitle = model.Title;
                    requestModel.PostOwner = HttpContext.Session.GetString("username");
                    
                    using (var target = new MemoryStream())
                    {
                        model.Image.CopyTo(target);
                        requestModel.Image = target.ToArray();
                    }


                    //HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("post/add", post).Result;
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            var url = "https://localhost:44305/api/post/add";
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                            var response = await client.PostAsJsonAsync(url, requestModel);
                            var s = response.StatusCode;

                            response.EnsureSuccessStatusCode();

                            if (response.IsSuccessStatusCode)
                            {
                                return RedirectToAction("Index");                                
                            }                                                       
                        }
                    }
                    catch (Exception e)
                    {

                        throw new Exception(e.Message);

                    }
                    

                }
            }
            return RedirectToAction("AddPost");
        }
    }
}
