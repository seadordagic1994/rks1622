using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Net.Http.Formatting;

namespace CondorExtreme3_API.Helper
{
    public class WebAPIHelper
    {
        public HttpClient Client { get; set; }

        public string Route { get; set; }


        public WebAPIHelper(string Uri, string Route = "")
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(Uri);
            this.Route = Route;
        }

        public HttpResponseMessage GetResponse(string routeExtension = "", object parameter = null)
        {
            string FinalRoute = Route;
            if (routeExtension.Length > 0)
                FinalRoute += "/" + routeExtension + "/";

            if (parameter != null)
                FinalRoute += parameter.ToString();

            return Client.GetAsync(FinalRoute).Result;
        }

        public HttpResponseMessage PostResponse(string jsonString, string routeExtension)
        {
            string FinalRoute = Route;
            if (routeExtension.Length > 0)
                FinalRoute += "/" + routeExtension + "/";

            return Client.PostAsync(FinalRoute, new StringContent(jsonString, Encoding.UTF8, "application/json")).Result;
        }
    }
}
