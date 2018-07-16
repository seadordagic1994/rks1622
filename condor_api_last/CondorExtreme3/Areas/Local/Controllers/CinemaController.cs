using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CondorExtreme3.DAL;
using CondorExtreme3.Helper;
using System.Collections;
using Newtonsoft.Json;
using CondorExtreme3.Tools;
using CondorExtreme3.ModelsLocalDB;

namespace CondorExtreme3.Areas.Local.Controllers
{
    
    public class CinemaController : Controller
    {
        private CondorDBContextChild _myVar;

        public CondorDBContextChild context
        {
            get {
                if (HttpContext.Session["ConnectionString"] == "")
                {  return _myVar; }
                if (_myVar == null)
                    _myVar = new CondorDBContextChild(HttpContext.Session["ConnectionString"].ToString());
                return _myVar; }
            set {  _myVar = value; }
        }

      

        // GET: Local/Cinema
        public ActionResult Index(string Reservation = "")
        {
            if (Reservation != "")
                ViewBag.Message = Reservation;
                    if (context == null)
               return Redirect(Url.Content("~/"));
            Cinema c = context.Cinemas.FirstOrDefault();

            return View(c);
        }
        public string Nule(string a)
        {
            if (a == "0")
                a = "00";
            return a;
        }
        public void getMovies(ProjectionVM projectionVM)
        {
            foreach (Projection p in projectionVM.Projections)
            {
                if (!projectionVM.d.ContainsKey(p.MovieID))
                    projectionVM.d[p.MovieID] = new List<string[]>();

                projectionVM.d[p.MovieID].Add(new string[]
                { p.TechnologyType.Name,
                  p.DateTimeStart.DayOfWeek.ToString(),
                  p.DateTimeStart.Hour.ToString(),
                  Nule(p.DateTimeStart.Minute.ToString()),
                  p.DateTimeStart.Year.ToString(),
                  p.DateTimeStart.Month.ToString(),
                  p.DateTimeStart.Day.ToString()
                });
            }

        }

        public ActionResult GetProjectionsSort(string cinema, string SortProjections)
        {
            int cinemaID;
            if (cinema == null)
                cinemaID = 0;
            else
                cinemaID = int.Parse(cinema);

            List<Projection> Projections = new List<Projection>();
            ProjectionVM projectionVM = new ProjectionVM();

            if (SortProjections == "Name")
            {
                projectionVM.Projections = context.Projections.Where(i => i.CinemaHall.CinemaID == cinemaID).OrderBy(x => x.Movie.MovieName).ToList();
                getMovies(projectionVM);
                projectionVM.Projections = projectionVM.Projections.Where(x => x.DateTimeStart > DateTime.Now.Subtract(new TimeSpan(0, 30, 0))).GroupBy(x => x.MovieID).Select(g => g.First()).Take(16).ToList();
                return PartialView("GetProjections", projectionVM);
            }
            else if (SortProjections == "Release")
            {
                projectionVM.Projections = context.Projections.Where(i => i.CinemaHall.CinemaID == cinemaID).OrderBy(x => x.Movie.ReleaseYear).ToList();
                getMovies(projectionVM);
                projectionVM.Projections = projectionVM.Projections.Where(x => x.DateTimeStart > DateTime.Now.Subtract(new TimeSpan(0, 30, 0))).GroupBy(x => x.MovieID).Select(g => g.First()).Take(16).ToList();
                return PartialView("GetProjections", projectionVM);
            }
            else if (SortProjections == "Duration")
            {
                projectionVM.Projections = context.Projections.Where(i => i.CinemaHall.CinemaID == cinemaID).OrderBy(x => x.Movie.DurationInMinutes).ToList();
                getMovies(projectionVM);
                projectionVM.Projections = projectionVM.Projections.Where(x => x.DateTimeStart > DateTime.Now.Subtract(new TimeSpan(0, 30, 0))).GroupBy(x => x.MovieID).Select(g => g.First()).Take(16).ToList();
                return PartialView("GetProjections", projectionVM);
            }

            projectionVM.Projections = context.Projections.Where(i => i.CinemaHall.CinemaID == cinemaID).ToList();
            getMovies(projectionVM);
            projectionVM.Projections = projectionVM.Projections.Where(x => x.DateTimeStart > DateTime.Now.Subtract(new TimeSpan(0, 30, 0))).GroupBy(x => x.MovieID).Select(g => g.First()).Take(16).ToList();

            return PartialView("GetProjections", projectionVM);
        }


        public ActionResult GetProjections(string cinema, string SortProjections,string search = "")
        {
            int cinemaID;
            if (cinema == null)
                cinemaID = 0;
            else
                cinemaID = int.Parse(cinema);
            List<Projection> Projections = new List<Projection>();
            ProjectionVM projectionVM = new ProjectionVM();

            if (SortProjections == "Name")
            {
                projectionVM.Projections = context.Projections.Where(i => i.CinemaHall.CinemaID == cinemaID).OrderBy(x => x.Movie.MovieName).ToList();
                getMovies(projectionVM);
                projectionVM.Projections = projectionVM.Projections.Where(x=> x.DateTimeStart > DateTime.Now.Subtract(new TimeSpan(0,30,0))).GroupBy(x => x.MovieID).Select(g => g.First()).Take(16).ToList();
                return PartialView("GetProjections", projectionVM);
            }
            else if (SortProjections == "Release")
            {
                projectionVM.Projections = context.Projections.Where(i => i.CinemaHall.CinemaID == cinemaID).OrderBy(x => x.Movie.ReleaseYear).ToList();
                getMovies(projectionVM);
                projectionVM.Projections = projectionVM.Projections.Where(x => x.DateTimeStart > DateTime.Now.Subtract(new TimeSpan(0, 30, 0))).GroupBy(x => x.MovieID).Select(g => g.First()).Take(16).ToList();
                return PartialView("GetProjections", projectionVM);
            }
            else if (SortProjections == "Duration")
            {
                projectionVM.Projections = context.Projections.Where(i => i.CinemaHall.CinemaID == cinemaID).OrderBy(x => x.Movie.DurationInMinutes).ToList();
                getMovies(projectionVM);
                projectionVM.Projections = projectionVM.Projections.Where(x => x.DateTimeStart > DateTime.Now.Subtract(new TimeSpan(0, 30, 0))).GroupBy(x => x.MovieID).Select(g => g.First()).Take(16).ToList();
                return PartialView("GetProjections", projectionVM);
            }


            if (cinemaID == 0)
                return PartialView("GetProjections", projectionVM);

            if (search == "") // vrati sve preko cinemaID
            {
                projectionVM.Projections = context.Projections.Where(i => i.CinemaHall.CinemaID == cinemaID).ToList();
                getMovies(projectionVM);
                projectionVM.Projections = projectionVM.Projections.Where(x => x.DateTimeStart > DateTime.Now.Subtract(new TimeSpan(0, 30, 0))).GroupBy(x => x.MovieID).Select(g => g.First()).Take(16).ToList();
            }
            else // vrati klasicni contains
            {
                projectionVM.Projections = context.Projections.Where(i => i.CinemaHall.CinemaID == cinemaID && i.Movie.MovieName.Contains(search)).ToList();
                getMovies(projectionVM);
                projectionVM.Projections = projectionVM.Projections.Where(x => x.DateTimeStart > DateTime.Now.Subtract(new TimeSpan(0, 30, 0))).GroupBy(x => x.MovieID).Select(g => g.First()).Take(16).ToList();

                if (projectionVM.Projections.Count() == 0) // pozovi algoritam
                {

                    projectionVM.Projections = context.Projections.Where(i => i.CinemaHall.CinemaID == cinemaID).ToList();
                    getMovies(projectionVM);
                    //da ne ponavlja filmove na naslovnoj stranici (postavi novi termin u projekciju)
                    projectionVM.Projections = projectionVM.Projections.Where(x => x.DateTimeStart > DateTime.Now.Subtract(new TimeSpan(0, 30, 0))).GroupBy(x => x.MovieID).Select(g => g.First()).Take(16).ToList();

                    List<string> lista = new List<string>();

                    foreach (Projection s in projectionVM.Projections)
                        lista.Add(s.Movie.MovieName.Trim() + '\n' + s.Movie.Synopsis.Trim());

                    string bestMatch = lista.ContextFind(search).Split('\n')[0];

                    projectionVM.Projections.Clear();

                    projectionVM.Projections.Add(context.Projections.Where
                                                (i => i.CinemaHall.CinemaID == cinemaID && i.Movie.MovieName.Contains(bestMatch)).First());
                    projectionVM.didUMean = true;
                }
            }

            //projectionVM.Projections = context.Projections.Where(i => i.CinemaHall.CinemaID == cinemaID && i.Movie.MovieName.Contains(search)).ToList();

            return PartialView("GetProjections", projectionVM);
        }

        [HttpPost]
        public ActionResult GetProjection(string jsonDictionary, int? projectionID)
        {
            ProjectionTimesVM model = new ProjectionTimesVM();
            model.Projections = context.Projections.Where(x => x.ProjectionID == projectionID).First();
            model.d = JsonConvert.DeserializeObject<Dictionary<int, List<string[]>>>(jsonDictionary);

            return PartialView("GetProjection", model);
        }

        [HttpPost]
        public ActionResult GetSeats(int MovieID, string type, string year, string mounth, string day, string hours, string minutes, string jsonDictionary)
        {
            SeatsVM model = new SeatsVM();
            model.d = JsonConvert.DeserializeObject<Dictionary<int, List<string[]>>>(jsonDictionary);

            int minute = int.Parse(minutes);
            int hour = int.Parse(hours);

            model.Projections = context.Projections.
                                Where(x => x.MovieID == MovieID &&
                                      x.TechnologyType.Name.ToString() == type &&
                                      x.DateTimeStart.Year.ToString() == year &&
                                      x.DateTimeStart.Month.ToString() == mounth &&
                                      x.DateTimeStart.Day.ToString() == day &&
                                      x.DateTimeStart.Hour == hour &&
                                      x.DateTimeStart.Minute == minute
                                      ).First();

            model.ProjectionSeat = context.ProjectionSeats.Where(x => x.ProjectionID == model.Projections.ProjectionID).ToList();
            model.ProjectionSeat = model.ProjectionSeat.OrderBy(x => x.Seat.SeatRowID).ToList();

            return PartialView("GetSeats", model);
        }

        [HttpPost]
        public JsonResult KeepSessionAlive()
        {

            return new JsonResult { Data = "Success" };
        }
        [HttpPost]
        public ActionResult MakeReservation()
        {
            return View("MakeReservation");
        }

    }
}