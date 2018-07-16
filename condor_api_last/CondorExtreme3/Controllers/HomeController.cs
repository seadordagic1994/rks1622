using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CondorExtreme3.Models;
using CondorExtreme3.DAL;
using System.Web.UI;
namespace CondorExtreme3.Controllers
{
    public class HomeController : Controller
    {
        CondorDBContext context = new CondorDBContext();


        // GET: Home
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Index()
        {

            HttpContext.Session["ConnectionString"] = "";
            HomeIndexVM model = new HomeIndexVM();
            model.Countries = context.Countries.Select(x => new SelectListItem
            {
                Value = x.CountryID.ToString(),
                Text = x.Name
            }).ToList();

            return View(model);
        }

        public ActionResult GetCityOverCountry(string country)
        {
            int CountryID = int.Parse(country);
            HomeIndexVM model = new HomeIndexVM();
            model.Cities = context.Cities.Where(x => x.CountryID == CountryID).Select(x => new SelectListItem
            {
                Value = x.CityID.ToString(),
                Text = x.Name
            }).ToList();

            return PartialView("GetCityOverCountry", model);
        }

        public ActionResult GetCinemaOverCity(string city)
        {
            int CityID = int.Parse(city);
            HomeIndexVM model = new HomeIndexVM();
            model.Cinemas = context.Cinemas.Where(x => x.Address.CityID == CityID).Select(x => new SelectListItem
            {
                Value = x.ConnectionString,
                Text = x.Name + ", " + x.Address.AddressLine1
            }).ToList();

            return PartialView("GetCinemaOverCity", model);
        }

        public ActionResult Redirection(string cinema)
        {
            HttpContext.Session["ConnectionString"] = cinema;

                      
            return RedirectToAction("../Local/Cinema");
        }
    }
}