using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CondorExtreme3.ModelsLocalDB;
using System.Web.Mvc;
using CondorExtreme3.DAL;
using System.ComponentModel.DataAnnotations;

namespace CondorExtreme3.Areas.Local.Models
{
    public class AddProjectionVM
    {
        public Projection Projection { get; set; } = new Projection();
        public List<Projection> Projections { get; set; } = new List<Projection>();
        public List<string> Traillers { get; set; } = new List<string>();

        [Required(ErrorMessage = "*")]
        public int GenreID { get; set; }       
        public List<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
        public int[] ActorsIDs { get; set; }
        public List<SelectListItem> Actors { get; set; } = new List<SelectListItem>();
        public int[] DirectorsIDs { get; set; }
        public List<SelectListItem> Directors { get; set; } = new List<SelectListItem>();
        public string IsCurrent { get; set; }
        public int MovieID { get; set; }
        public List<SelectListItem> Movies { get; set; } = new List<SelectListItem>();
        public int CinemaHallID { get; set; }
        public List<SelectListItem> CinemaHalls { get; set; } = new List<SelectListItem>();
        public DateTime DateTimeStartID { get; set; }
        public List<SelectListItem> DateTimeStarts { get; set; } = new List<SelectListItem>();
        public int TechTypeID { get; set; }
        public List<SelectListItem> TechTypes { get; set; } = new List<SelectListItem>();
        public string SuccessDescription { get; set; }

        public AddProjectionVM()
        {
            using (CondorDBContextChild principal = new CondorDBContextChild(HttpContext.Current.Session["ConnectionString"].ToString()))
            {
                Genres = principal.Genres.Select(x => new SelectListItem
                {
                    Text = x.GenreName,
                    Value = x.GenreID.ToString()
                }).ToList();

                Actors = principal.Actors.Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.ActorID.ToString()
                }).ToList();

                Directors = principal.Directors.Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.DirectorID.ToString()
                }).ToList();

                Movies = principal.Movies.Select(x => new SelectListItem
                {
                    Text = x.OriginalName + "(" + x.MovieName + ")",
                    Value = x.MovieID.ToString()
                }).ToList();

                CinemaHalls = principal.CinemaHalls.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.CinemaHallID.ToString()
                }).ToList();

                DateTimeStarts = principal.DefinedDateTimes.Select(x => new SelectListItem
                {
                    Text = x.DateTimeStart.ToString(),
                    Value = x.DateTimeStart.ToString()
                }).ToList();

                TechTypes = principal.TechnologyTypes.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.TechnologyTypeID.ToString()
                }).ToList();
            }
        }
    }
}