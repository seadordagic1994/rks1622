using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CondorExtreme3.ModelsLocalDB;
using CondorExtreme3.DAL;
using CondorExtreme3.Helper;


namespace CondorExtreme3.Controllers
{
    public class LoginController : Controller
    {
        private CondorDBContextChild _myVar;

        public CondorDBContextChild principal
        {
            get
            {
                if (HttpContext.Session["ConnectionString"] == "")
                { return _myVar; }
                if (_myVar == null)
                    _myVar = new CondorDBContextChild(HttpContext.Session["ConnectionString"].ToString());
                return _myVar;
            }
            set { _myVar = value; }
        }
        // GET: Login
        public ActionResult EmployeeLogin()
        {
            if (principal == null)
                return Redirect(Url.Content("~/"));

            Employee Model = new Employee();
            return View("EmployeeLoginView", Model);
        }

        public ActionResult VerifyRequest(Employee model)
        {
            if (!ModelState.IsValid)
            {
                return View("EmployeeLoginView", model);
            }
            Employee e = principal.Employees
                .Where(x => x.Username == model.Username && x.Password == model.Password).SingleOrDefault();

            if (e == null)
            {
                return RedirectToAction("EmployeeLogin");
            }

            Authentication.StartNewSession(e, HttpContext, false);

            Dictionary<int, string> roles = Session["paths"] as Dictionary<int, string>;
            return RedirectToAction(roles.First().Value);
        }
        public ActionResult Logout()
        {
            //Authentication.StartNewSession(null, HttpContext, true);
            Authentication.EndSession(Authentication.GetLoggedEmployee(HttpContext), HttpContext);
            return RedirectToAction("../Local/Cinema/Index");
        }
    }
}