using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CondorExtreme3.ModelsLocalDB;
using CondorExtreme3.DAL;

namespace CondorExtreme3.Helper
{
    public class Autorization : FilterAttribute, IAuthorizationFilter
    {
        private readonly Permissions _roles;

        public Autorization(Permissions roles)
        {
            _roles = roles;
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {


            Employee E = Authentication.GetLoggedEmployee(filterContext.HttpContext);
            Dictionary<int, string> roles = HttpContext.Current.Session["paths"] as Dictionary<int, string>;

            if (E == null)
            {
                filterContext.HttpContext.Response.Redirect("/Login/EmployeeLogin");
                return;
            }



            if (roles.ContainsKey((int)_roles))
                return;

            filterContext.HttpContext.Response.Redirect(roles.First().Value);

        }

        public enum Permissions
        {
            Director = 1,
            ProjectionManager = 2,
            Employee = 3
        }
    }
}