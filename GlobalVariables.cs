using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace KumportWeb
{
    public class GlobalVariables
    {
        public static HttpClient WebApiClient = new HttpClient();

        static GlobalVariables()
        {
            WebApiClient.BaseAddress = new Uri("https://localhost:44305/api/");
            WebApiClient.DefaultRequestHeaders.Clear();            
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
           
        }
    }
}
