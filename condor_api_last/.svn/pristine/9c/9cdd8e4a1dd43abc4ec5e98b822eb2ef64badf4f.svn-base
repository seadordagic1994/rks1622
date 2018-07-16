using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CondorExtreme3.ModelsLocalDB;
using CondorExtreme3.DAL;

namespace CondorExtreme3.Helper
{
    public class Authentication
    {
        private const string loggedUser = "activeUser";

        public static void StartNewSession(Employee Em, HttpContextBase Context, bool RememberMe)
        {
            if (Em == null)
            {
                Context.Session.Add(loggedUser, Em);
                return;
            }
            using (CondorDBContextChild principal = new CondorDBContextChild(HttpContext.Current.Session["ConnectionString"].ToString()))
            {
                List<EmployeeRole> EmployeeRoles = new List<EmployeeRole>();
                EmployeeRoles = principal.EmployeesRoles.Where(x => x.EmployeeID == Em.EmployeeID).ToList();
                EmployeeRoles = EmployeeRoles.OrderBy(x => x.RoleID).ToList();

                Dictionary<string, int> RoleName = new Dictionary<string, int>();

                foreach (Role r in principal.Roles)
                    RoleName.Add(r.RoleName, r.RoleID);

                Dictionary<int, string> paths = new Dictionary<int, string>
                {
                    {RoleName["Director"], "/../Local/Director" },
                    {RoleName["ProjectionManager"], "/../Local/ProjectionManager" },
                    {RoleName["Employee"], "/../Local/Employee" }
                };

                for (int i = 0; i < paths.Count; i++)
                    if (!EmployeeRoles.Where(e => e.RoleID == paths.Keys.ElementAt(i)).Any())
                    { paths.Remove(paths.Keys.ElementAt(i)); i--; }


                Context.Session.Add(loggedUser, Em);
                

                HttpContext.Current.Session["paths"] = paths as Dictionary<int, string>;

               
                if (RememberMe)
                {
                    HttpCookie cookie = new HttpCookie("CondorExtreme3Cookie", Em.EmployeeID.ToString());
                    cookie.Expires = DateTime.Now.AddDays(5);
                    Context.Response.Cookies.Add(cookie);
                }
            }
        }

        internal static object GetLoggedEmployee(object context)
        {
            throw new NotImplementedException();
        }

        public static Employee GetLoggedEmployee(HttpContextBase context)
        {
            Employee Em = (Employee)context.Session[loggedUser];

            if (Em != null)
                return Em;

            HttpCookie cookie = context.Request.Cookies.Get("CondorExtreme3Cookie");

            if (cookie == null)
                return null;

            int EiD;
            try
            {
                EiD = Int32.Parse(cookie.Value);
            }
            catch
            {
                return null;

            }
            using (CondorDBContextChild principal = new CondorDBContextChild(HttpContext.Current.Session["ConnectionString"].ToString()))
            {
                Employee e = principal.Employees
                    .Where(x => x.EmployeeID == EiD)
                    .FirstOrDefault();

                StartNewSession(e, context, true);
                return e;
            }
        }


        public static void EndSession(Employee Em, HttpContextBase Context)
        {
            Context.Session[loggedUser] = null;
        }
    }
}