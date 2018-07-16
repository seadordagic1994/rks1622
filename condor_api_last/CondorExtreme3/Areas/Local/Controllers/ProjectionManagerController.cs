using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CondorExtreme3.Helper;
using CondorExtreme3.Areas.Local.Models;
using CondorExtreme3.DAL;
using CondorExtreme3.ModelsLocalDB;
using Newtonsoft.Json;
using CondorExtreme3.Tools;

namespace CondorExtreme3.Areas.Local.Controllers
{
    [Autorization(Autorization.Permissions.ProjectionManager)]
    public class ProjectionManagerController : Controller
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

        // GET: Projections/ProjectionManager
        public ActionResult Index()
        {
            if (principal == null)
                return Redirect(Url.Content("~/"));

            return RedirectToAction("GetCinemaHalls", new { layout = "~/Areas/Local/Views/Shared/_LayoutProjectionManager.cshtml" });
        }

        // DODAVANJE KINA (VIŠE NIJE OBUHVAĆENO U OVOM SEGMENTU)
        //[HttpPost]
        //public ActionResult AddCinema()
        //{
        //    AddCinemaVM cinema = new AddCinemaVM();
        //    cinema.Countries = principal.Country.Select(x => new SelectListItem()
        //    {
        //        Text = x.Name,
        //        Value = x.CountryID.ToString()
        //    }).ToList();

        //    cinema.Cities = principal.Cities.Select(x => new SelectListItem()
        //    {
        //        Text = x.Name,
        //        Value = x.CityID.ToString()
        //    }).ToList();

        //    cinema.Addreses = principal.Addresses.Select(x => new SelectListItem()
        //    {
        //        Text = x.AddressLine1,
        //        Value = x.AddressID.ToString()
        //    }).ToList();

        //    return PartialView("AddCinema", cinema);
        //}

        public ActionResult AddCinemaHall()
        {
            AddCinemaVM model = new AddCinemaVM();
            model.Cinema = principal.Cinemas.First();

            return PartialView("AddCinemaHall", model);
        }

        public ActionResult SubmitCinemaHall(AddCinemaVM model)
        {
            // if (!ModelState.IsValid)
            //    return PartialView("AddCinemaHall", model);

            CinemaHall ch = new CinemaHall();
            ch.CinemaID = model.Cinema.CinemaID;
            ch.Name = model.CinemaHall.Name;
            ch.IsDeleted = false;
            principal.CinemaHalls.Add(ch);
            principal.SaveChanges();

            model.CinemaHall = ch;

            return PartialView("AddTechType", model);
        }

        public ActionResult SubmitTechType(AddCinemaVM model, string[] check)
        {

            foreach (string s in check)
            {
                CinemaHallTechnologyType tt = new CinemaHallTechnologyType();

                tt.CinemaHallID = model.CinemaHall.CinemaHallID;

                tt.TechnologyTypeID = int.Parse(s);
                tt.IsDeleted = false;
                principal.CinemaHallTechnologyTypes.Add(tt);
                principal.SaveChanges();
            }


            return RedirectToAction("GetCinemaHalls", new { layout = "~/Areas/Local/Views/Shared/_LayoutProjectionManager.cshtml" });
        }

        public ActionResult GetCinemaHalls(string layout = null)
        {
            CinemaVM cinemaHalls = new CinemaVM();
            cinemaHalls.Cinemas = principal.Cinemas.ToList();
            cinemaHalls.CinemaHalls = principal.CinemaHalls.ToList();

            foreach (CinemaHall ch in cinemaHalls.CinemaHalls)
            {
                //detalji dvorane: broj sjedišta i tipovi tehnologija
                cinemaHalls.NumberOfSeats.Add(principal.Seats.Where(x => x.CinemaHallID == ch.CinemaHallID && !x.IsDeleted).Count());
                cinemaHalls.TechTypes.Add(
                                           ch.CinemaHallID,
                                           principal.CinemaHallTechnologyTypes.Where(x => x.CinemaHallID == ch.CinemaHallID).Select(y => y.TechnologyType.Name).ToList()
                                         );
            }

            return View("GetCinemas", layout, cinemaHalls);
        }

        public ActionResult GetSeats(int? cinemaHallID)
        {
            PMSeatsVM model = new PMSeatsVM();

            foreach (SeatRow sr in model.SeatRow)
                model.Seats[sr.SeatRowID] = new List<string>();


            model.CinemaHall = principal.CinemaHalls.Where(x => x.CinemaHallID == cinemaHallID).First();
            List<Seat> seats = principal.Seats.Where(x => x.CinemaHallID == cinemaHallID && !x.IsDeleted).ToList();

            foreach (Seat s in seats)
                model.Seats[s.SeatRowID].Add(s.SeatColumnID.ToString());

            if (cinemaHallID == null)
                RedirectToAction("Index");

            return PartialView("GetSeats", model);
        }

        public ActionResult GetSeatsDictionary(PMSeatsVM model, int FillSeat, string SeatRowID, string JsonDict)
        {

            model.Seats = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(JsonDict);


            // if (!model.Seats.ContainsKey(SeatRowID))
            //   model.Seats[SeatRowID] = new List<string>();

            int s = model.Seats[SeatRowID].Count();

            if (FillSeat > s)
                for (int i = s, j = 1; i < FillSeat; i++, j++)
                    model.Seats[SeatRowID].Add((s + j).ToString());

            if (FillSeat < s)
                for (int i = s; i > FillSeat; i--)
                    model.Seats[SeatRowID].RemoveAt(i - 1);

            return PartialView("GetSeatsDictionary", model);
        }

        public ActionResult AcceptSeats(int CinemaHallID, string JsonDict)
        {
            Seat AddSeat = new Seat();
            var NewSeats = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(JsonDict);
            List<Seat> Seats = principal.Seats.Where(x => x.CinemaHallID == CinemaHallID && !x.IsDeleted).OrderBy(x=> x.SeatRowID).ToList();
            

            if (Seats.Count != 0)
            {
                //char Row = 'A';
                for(int j = 0; j < Seats.Count; j++)
                {
                    if (NewSeats.ContainsKey(Seats[j].SeatRowID))
                    {
                        int size = Seats.Where(x => x.SeatRowID == Seats[j].SeatRowID).Count();
                        if (NewSeats[Seats[j].SeatRowID].Count > size)
                        {
                           
                            for (int i = size; i < NewSeats[Seats[j].SeatRowID].Count; i++)
                            {
                                AddSeat.CinemaHallID = CinemaHallID;
                                AddSeat.SeatRowID = Seats[j].SeatRowID;
                                AddSeat.SeatColumnID = i + 1;
                                AddSeat.IsDeleted = false;
                                AddSeat.SeatTypeID = 1;
                                principal.Seats.Add(AddSeat);
                                principal.SaveChanges();                                  
                            }
                            j = j + size - 1;
                        }

                        if (NewSeats[Seats[j].SeatRowID].Count < size)
                        {
                            for (int i = Seats.Where(x => x.SeatRowID == Seats[j].SeatRowID).Count(); i > NewSeats[Seats[j].SeatRowID].Count; i--)
                            {
                                string rowID = Seats[j].SeatRowID;
                                AddSeat = principal.Seats.Where(x => x.SeatRowID == rowID && x.SeatColumnID == i && !x.IsDeleted).FirstOrDefault();
                                AddSeat.IsDeleted = true;
                                principal.SaveChanges();
                            }
                            j = j + size - 1;
                        }
                    }
                    //Row++;
                }
            }
            else
            {
                foreach (var ns in NewSeats)
                {
                    foreach (string s in ns.Value)
                    {
                        AddSeat.CinemaHallID = CinemaHallID;
                        AddSeat.SeatRowID = ns.Key;
                        AddSeat.SeatColumnID = int.Parse(s);
                        AddSeat.IsDeleted = false;
                        AddSeat.SeatTypeID = 1;
                        principal.Seats.Add(AddSeat);
                        principal.SaveChanges();
                    }
                }
            }
            return RedirectToAction("GetCinemaHalls", new { layout = "~/Areas/Local/Views/Shared/_LayoutProjectionManager.cshtml" });
        }
        
        public ActionResult GetProjections(string layout = null )
        {
            AddProjectionVM model = new AddProjectionVM();
            model.Projections = principal.Projections.ToList();
            return View("GetProjections", layout, model);
        }
        
        public ActionResult AddProjection(string descript = null)
        {
            AddProjectionVM model = new AddProjectionVM();
            model.SuccessDescription = descript;

            return View("AddProjection",model);
        }
        
        public ActionResult ShowTrailler(AddProjectionVM model, string ButtonType)
        {
            //if (!ModelState.IsValid)
            //    return RedirectToAction("AddProjection", model); 
            if(ButtonType != "next")
            model.Traillers = Algorithm.SuggestTrailerLinks(model.Projection.Movie.MovieName, 3);

            if (ButtonType == "save") {
               Movie m = model.Projection.Movie;
                m.IsDeleted = false;
                m.GenreID = model.GenreID;
                m.Genre = principal.Genres.Where(x => x.GenreID == model.GenreID).First();
                m.Trailler = model.Traillers[0];
                principal.Movies.Add(m);
                principal.SaveChanges();

                foreach (int i in model.ActorsIDs)
                {
                    MovieRole mr = new MovieRole();
                    mr.MovieID = m.MovieID;
                    mr.ActorID = i;
                    mr.Actor = principal.Actors.Where(x => x.ActorID == i).First();
                    mr.IsDeleted = false;
                    principal.MovieRoles.Add(mr);
                    principal.SaveChanges();
                }

                foreach (int i in model.DirectorsIDs)
                {
                    MovieDirection md = new MovieDirection();
                    md.MovieID = m.MovieID;
                    md.DirectorID = i;
                    md.Director = principal.Directors.Where(x => x.DirectorID == i).First();
                    md.IsDeleted = false;
                    principal.MovieDirections.Add(md);
                    principal.SaveChanges();
                    model.Projection.Movie.MovieID = md.MovieID;
                }

                model.Movies = principal.Movies.Select(x => new SelectListItem
                {
                    Text = x.OriginalName + "(" + x.MovieName + ")",
                    Value = x.MovieID.ToString()
                }).ToList();

                model.Movies = model.Movies.OrderByDescending(x => int.Parse(x.Value)).ToList();

                model.Projection.Movie.MovieRoles = principal.MovieRoles.Where(x=> x.MovieID == m.MovieID).ToList();
                model.Projection.Movie.MovieDirections = principal.MovieDirections.Where(x => x.MovieID == m.MovieID).ToList();

                model.IsCurrent = principal.Movies.Where(x => x.MovieID == m.MovieID).Select(x => x.IsCurrent).ToString();
                model.IsCurrent = (model.IsCurrent == "0") ? "NO" : "YES";

                return PartialView("SetProjection", model);
            }

            if (ButtonType == "next")
            {
                return PartialView("SetProjection", model);
            }

                //model.Projection.Movie.Genre.GenreName = genreName;
                //model.IsCurrent = (principal.Movies.Where(x => x.IsCurrent.ToString() == model.IsCurrent).ToString() == "0") ? "NO" : "YES";
                return View("ShowTrailler", model);
        }

        public ActionResult ShowMovie(AddProjectionVM model)
        {
            model.Projection.Movie = principal.Movies.Where(x => x.MovieID == model.MovieID).First();
            model.IsCurrent = principal.Movies.Where(x => x.MovieID == model.Projection.Movie.MovieID).Select(x => x.IsCurrent).ToString();
            model.IsCurrent = (model.IsCurrent == "0") ? "NO" : "YES";
            return PartialView("ShowMovie", model);
        }

        public ActionResult SubmitProjection(AddProjectionVM model, string btnType)
        {
            //if (!ModelState.IsValid)
            //    return     
            DefinedDateTime ddt = new DefinedDateTime();
            if (principal.DefinedDateTimes.Where(x => x.DateTimeStart == model.Projection.DateTimeStart).FirstOrDefault() == null)
            {
                ddt.DateTimeStart = model.Projection.DateTimeStart;
                ddt.IsDeleted = false;
                principal.DefinedDateTimes.Add(ddt);
                principal.SaveChanges();
            }
            else
                ddt = principal.DefinedDateTimes.Where(x => x.DateTimeStart == model.Projection.DateTimeStart).First();


                Projection p = new Projection();
                p.CinemaHallID = model.CinemaHallID;
                p.CinemaHall = principal.CinemaHalls.Where(x => x.CinemaHallID == model.CinemaHallID).First();
                p.DateTimeEnd = model.Projection.DateTimeEnd;
                p.DateTimeStart = ddt.DateTimeStart;
                p.MovieID = model.Projection.Movie.MovieID;
                p.Movie = principal.Movies.Where(x => x.MovieID == model.Projection.Movie.MovieID).First();
                p.TechnologyTypeID = model.TechTypeID;
                p.TechnologyType = principal.TechnologyTypes.Where(x => x.TechnologyTypeID == model.TechTypeID).First();
                p.TicketPrice = model.Projection.TicketPrice;
                p.IsDeleted = false;
                principal.Projections.Add(p);
                principal.SaveChanges();

            foreach (Seat s in principal.Seats.Where(x => x.CinemaHallID == p.CinemaHallID && !x.IsDeleted).ToList())
            {
                ProjectionSeat ps = new ProjectionSeat();
                ps.IsDeleted = false;
                ps.IsReserved = false;
                ps.Seat = s;
                ps.SeatID = s.SeatID;
                ps.ProjectionID = p.ProjectionID;
                ps.Projection = p;
                principal.ProjectionSeats.Add(ps);
                principal.SaveChanges();
            }
                return RedirectToAction("AddProjection", new { descript = "New projection successfuly added !" });
           
        }
        [HttpPost]
        public ActionResult GetCinemaHallsForProjection(string dateStart, string dateEnd, string ProjectionMovieID, string MovieID)
        {
            AddProjectionVM model1 = new AddProjectionVM();
            int ID = int.Parse(MovieID);

            model1.Projection.Movie = principal.Movies.Where(x => x.MovieID == ID).First();
            model1.Projection.DateTimeStart = DateTime.Parse(dateStart);
            model1.Projection.DateTimeEnd = DateTime.Parse(dateEnd);

            List<Projection> p = principal.Projections.ToList();

            model1.CinemaHalls = principal.CinemaHalls.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.CinemaHallID.ToString()
            }).ToList();

            //filtriraj dvorane koje su vec zauzete 
            foreach (Projection pro in p)
                if ((model1.Projection.DateTimeStart > pro.DateTimeStart && model1.Projection.DateTimeStart < pro.DateTimeEnd) ||
                   (model1.Projection.DateTimeEnd > pro.DateTimeStart && model1.Projection.DateTimeEnd < pro.DateTimeEnd) ||
                   ((model1.Projection.DateTimeStart > pro.DateTimeStart && model1.Projection.DateTimeStart < pro.DateTimeEnd) &&
                   (model1.Projection.DateTimeEnd > pro.DateTimeStart && model1.Projection.DateTimeEnd < pro.DateTimeEnd)))
                    model1.CinemaHalls.Remove(model1.CinemaHalls.Where(x => x.Value == pro.CinemaHall.CinemaHallID.ToString()).Single());

            model1.TechTypes = principal.TechnologyTypes.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.TechnologyTypeID.ToString()
            }).ToList();


            return PartialView("GetCinemaHallsForProjection", model1);
        }

        public ActionResult ImageUpload(HttpPostedFileBase image, string MovieID)
        {
            int ID = int.Parse(MovieID);
            if (image.ContentLength > 0 && !System.IO.File.Exists(Server.MapPath("~/Pictures/" + image.FileName)))
            {
                image.SaveAs(Server.MapPath("~/Pictures/" + image.FileName));
                principal.Movies.Where(x => x.MovieID == ID).First().Picture = "/Pictures/" + image.FileName;
                principal.SaveChanges();
            }
            return RedirectToAction("GetProjections", new {layout = "~/Areas/Local/Views/Shared/_LayoutProjectionManager.cshtml" } );
        }


        [HttpPost]
        public JsonResult KeepSessionAlive()
        {

            return new JsonResult { Data = "Success" };
        }


    }
}

//Seat ss = new Seat();
//ss.CinemaHallID = model.CinemaHall.CinemaHallID;
//ss.SeatColumnID = FillSeat;
//ss.SeatRowID = SeatRowID;
//ss.SeatTypeID = 0;
//principal.Seats.Add(ss);
//principal.SaveChanges();
//List<Seat> seats = principal.Seats.Where(x => x.CinemaHallID == model.CinemaHall.CinemaHallID).ToList();