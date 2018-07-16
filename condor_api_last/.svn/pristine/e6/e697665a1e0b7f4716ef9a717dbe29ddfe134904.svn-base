using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CondorExtreme3.ModelsLocalDB;
using CondorExtreme3.DAL;
using System.ComponentModel.DataAnnotations;

namespace CondorExtreme3.Helper
{
    public class RedirectToCinema : FilterAttribute, IAuthorizationFilter
    {
        public RedirectToCinema(string n){ }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (HttpContext.Current.Session["ConnectionString"].ToString() == "")
            { filterContext.HttpContext.Response.Redirect("../../Home/Index"); return; }

        }
    }
}