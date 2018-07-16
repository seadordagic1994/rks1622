using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CondorExtreme3.ModelsLocalDB;
using CondorExtreme3.DAL;
using CondorExtreme3.Helper;

namespace CondorExtreme3.ModelsLocalDB
{
   
    public class IndexVM
    {
        public int CountryID { get; set; }
        public int CityID { get; set; }
        public int CinemaID { get; set; }
        public List<SelectListItem> Countries { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Cities { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Cinemas { get; set; } = new List<SelectListItem>();

        public IndexVM(List<City> Cities, List<Cinema> Cinemas = null, List<Projection> Projection = null)
        {
            using (CondorDBContext context = new CondorDBContext())
            {
                this.Cities.AddRange(Cities.Select(x => new SelectListItem
                {
                    Value = x.CityID.ToString(),
                    Text = x.Name
                }).ToList());

                this.Cinemas.AddRange(Cinemas.Select(x => new SelectListItem
                {
                    Value = x.CinemaID.ToString(),
                    Text = x.Name + " " + x.Address.AddressLine1
                }).ToList());
            }

          
        }
    }
}