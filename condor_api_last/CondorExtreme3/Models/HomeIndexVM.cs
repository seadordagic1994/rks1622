using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CondorExtreme3.Models;
using CondorExtreme3.DAL;
using CondorExtreme3.Helper;

namespace CondorExtreme3.Models
{
   
    public class HomeIndexVM
    {
        public int CountryID { get; set; }
        public int CityID { get; set; }
        public int CinemaID { get; set; }
        public List<SelectListItem> Countries { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Cities { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Cinemas { get; set; } = new List<SelectListItem>();

        public HomeIndexVM()
        {
            using (CondorDBContext context = new CondorDBContext())
            {
                Countries = context.Countries.Select(x => new SelectListItem
                {
                    Value = x.CountryID.ToString(),
                    Text = x.Name
                }).ToList();
            }

          
        }
    }
}