using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CondorExtreme3.ModelsLocalDB;
using System.Web.Mvc;
using CondorExtreme3.DAL;
namespace CondorExtreme3.Areas.Local.Models
{
    public class AddCinemaVM
    {

        public int CountryID { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public int CityID { get; set; }
        public List<SelectListItem> Cities { get; set; } = new List<SelectListItem>();
        public int AddressID { get; set; }
        public List<SelectListItem> Addreses { get; set; } = new List<SelectListItem>();
        public int CinemaID { get; set; }
        public Cinema Cinema { get; set; } = new Cinema();
        public CinemaHall CinemaHall { get; set; } = new CinemaHall();

        public List<TechnologyType> TechTypes { get; set; } = new List<TechnologyType>();

        private CondorDBContextChild _myVar;


        public AddCinemaVM()
        {
            CondorDBContextChild principal = new CondorDBContextChild(HttpContext.Current.Session["ConnectionString"].ToString());

            Countries = principal.Country.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.CountryID.ToString()
            }).ToList();

            Cities = principal.Cities.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.CityID.ToString()
            }).ToList();

            Addreses = principal.Addresses.Select(x => new SelectListItem()
            {
                Text = x.AddressLine1,
                Value = x.AddressID.ToString()
            }).ToList();

            TechTypes = principal.TechnologyTypes.ToList();
        }
     }
}