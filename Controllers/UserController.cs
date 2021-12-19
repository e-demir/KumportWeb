using Kumport.Common.RequestModels;
using Kumport.Common.ResponseModels;
using KumportWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace KumportWeb.Controllers
{
    public class UserController : Controller
    {
     
        [HttpGet]
        public async Task<IActionResult> Info()
        {
            var token = HttpContext.Session.GetString("token");
            var username = HttpContext.Session.GetString("username");
            var viewModel = new UserInfoViewModel();
            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(username))
            {
                var requestModel = new UserInfoRequestModel();
                requestModel.Username = username;

                try
                {
                    using (var client = new HttpClient())
                    {
                        var url = "https://localhost:44305/api/user/info";
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                        var response = await client.PostAsJsonAsync(url, requestModel);
                        var modelRes = response.Content.ReadAsAsync<UserInfoResponseModel>().Result;
                        if (modelRes.IsSuccesfull)
                        {
                            viewModel.Username = modelRes.Username;
                            viewModel.Email = modelRes.Email;
                            viewModel.Posts = new List<PostModel>();
                            if (modelRes.Posts.Count > 0)
                            {
                                foreach (var item in modelRes.Posts)
                                {
                                    var post = new PostModel();
                                    post.CreatedOn = item.CreatedOn;
                                    post.FileType = item.FileType;
                                    post.Image = item.Image;
                                    post.PostOwner = item.PostOwner;
                                    post.PostTitle = item.PostTitle;
                                    viewModel.Posts.Add(post);
                                }
                             
                            }
                            return View(viewModel);

                        }
                        else
                        {
                            return RedirectToAction("Index", "Posts");
                        }                        
                    }
                }
                catch (Exception e)
                {

                    throw new Exception(e.Message);

                }
            }
            else
            {
                return View("Views/Auth/UnAuth.cshtml");
            }                       
        }
    }
}
