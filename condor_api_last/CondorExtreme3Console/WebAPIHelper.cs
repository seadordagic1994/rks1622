﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CondorExtreme3Console
{
    public class WebAPIHelper
    {
        public HttpClient Client { get; set; }

        public string Route { get; set; }

        public static string ApiUri
        {

            get
            {
                return "http://localhost:61732/";
            }
            //get
            //{
            //    return "https://api.rs1122.app.fit.ba/";
            //}
        }



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
            {
                if (routeExtension.Length > 0)
                    FinalRoute += parameter.ToString();
                else FinalRoute += "/" + parameter.ToString();
            }
            return Client.GetAsync(FinalRoute).Result;
        }

        public HttpResponseMessage PostResponse(object obj, string routeExtension = "")
        {
            string FinalRoute = Route;
            if (routeExtension.Length > 0)
                FinalRoute += "/" + routeExtension + "/";
            return Client.PostAsJsonAsync(FinalRoute, obj).Result;
        }

        public HttpResponseMessage PutResponse(object obj, string routeExtension = "")
        {
            string FinalRoute = Route;
            if (routeExtension.Length > 0)
                FinalRoute += "/" + routeExtension + "/";
            return Client.PutAsJsonAsync(FinalRoute, obj).Result;
        }

        public HttpResponseMessage DeleteResponse(string routeExtension = "", object parameter = null)
        {
            string FinalRoute = Route;
            if (routeExtension.Length > 0)
                FinalRoute += "/" + routeExtension + "/";
            if (parameter != null)
            {
                if (routeExtension.Length > 0)
                    FinalRoute += parameter.ToString();
                else FinalRoute += "/" + parameter.ToString();
            }
            return Client.DeleteAsync(FinalRoute).Result;
        }


    }
}
