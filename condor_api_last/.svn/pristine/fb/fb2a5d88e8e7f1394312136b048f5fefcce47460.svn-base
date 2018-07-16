using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CondorExtreme3.Helper;

namespace CondorExtreme3.Areas.Local.Controllers
{
    [Autorization(Autorization.Permissions.Employee)]
    public class EmployeeController : Controller
    {
        // GET: Local/Employee
        public ActionResult Index()
        {
            return View("Index", "~/Areas/Local/Views/Shared/_LayoutEmployee.cshtml");
        }
    }
}