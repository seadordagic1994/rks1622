using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Http;
using CondorExtreme3_API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CondorExtreme3_API.Controllers
{
    [RoutePrefix("api/Projections")]
    public class AdminProjectionsController : ApiController
    {
        public CondorDBXEntities context = new CondorDBXEntities();

        [Route("GetGenres")]
        public IHttpActionResult GetGenres()
        {
            List<Genres> g = context.Genres.ToList();
            if (g == null)
            {
                return NotFound();
            }
            return Ok(g.Select(x => new
            { GenreID = x.GenreID,
                Name = x.Name,
            }));
        }
        [Route("GetActors")]
        public IHttpActionResult GetActors()
        {
            List<Actors> g = context.Actors.ToList();
            if (g == null)
            {
                return NotFound();
            }
            return Ok(g.Select(x => new
            {
                ActorID = x.ActorID,
                Name = x.FirstName + " " + x.LastName,
            }));
        }
        [Route("GetDirectors")]
        public IHttpActionResult GetDirectors()
        {
            List<MovieDirectors> g = context.MovieDirectors.ToList();
            if (g == null)
            {
                return NotFound();
            }
            return Ok(g.Select(x => new
            {
                DirectorId = x.MovieDirectorID,
                Name = x.FirstName + " " + x.LastName,
            }));
        }
        [Route("GetAge")]
        public IHttpActionResult GetAge()
        {
            List<AgeRestrictions> g = context.AgeRestrictions.ToList();
            if (g == null)
            {
                return NotFound();
            }
            return Ok(g.Select(x => new
            {
                AgeRestrictionID = x.AgeRestrictionID,
                Name = x.Name,
            }));
        }
        [Route("GetTechnologyTypes")]
        public IHttpActionResult GetTechnologyType()
        {
            List<TechnologyTypes> g = context.TechnologyTypes.ToList();
            if (g == null)
            {
                return NotFound();
            }
            return Ok(g.Select(x => new
            {
                Id = x.TechnologyTypeID,
                Name = x.Name,
            }));
        }

        [Route("GetCinemaHallsSingle/{cinemaId}")]
        public IHttpActionResult GetCinemaHallsSingle(int cinemaId)
        {
            List<CinemaHalls> g = context.CinemaHalls.Where(x=> x.CinemaID == cinemaId && !x.IsDeleted).ToList();
            if (g == null)
            {
                return NotFound();
            }
            return Ok(g.Select(x => new
            {
                Id = x.CinemaHallID,
                Name = x.Name,
            }));
        }
        [HttpPut]
        [Route("AddSeats")]
        public IHttpActionResult AddSeats(dynamic seats)
        {
            int cinemaHallId = int.Parse(seats.cinemaHallId.ToString());

            Dictionary<int, List<int>> dictSeats = JsonConvert.DeserializeObject<Dictionary<int, List<int>>>(seats.curSeats.ToString());

            if (dictSeats == null)
            {
                return NotFound();
            }

            foreach (Seats s in context.Seats.Where(x => x.CinemaHallID == cinemaHallId && !x.IsDeleted).ToList())
            {
                if (dictSeats.ContainsKey(s.SeatRowID))
                {
                    if (!dictSeats[s.SeatRowID].Contains(s.SeatColumnID))
                    {
                        context.Seats.Where(x => x.SeatRowID == s.SeatRowID && x.SeatColumnID == s.SeatColumnID && x.CinemaHallID == cinemaHallId && !x.IsDeleted).FirstOrDefault().IsDeleted = true;
                        context.SaveChanges();
                    }
                }
            }

            foreach (KeyValuePair<int, List<int>> kvp in dictSeats)
            {
                if (context.Seats.Where(x => x.CinemaHallID == cinemaHallId && x.SeatRowID == kvp.Key).FirstOrDefault() != null)
                {
                    foreach (int i in kvp.Value)
                        if (context.Seats.Where(x => x.CinemaHallID == cinemaHallId && x.SeatRowID == kvp.Key && x.SeatColumnID == i && !x.IsDeleted).FirstOrDefault() == null)
                        {
                            context.Seats.Add(new Seats { CinemaHallID = cinemaHallId, SeatRowID = kvp.Key, SeatColumnID = i, IsDeleted = false });
                            context.SaveChanges();
                        }

                }

            }


            return Ok();
        }

        [Route("GetSeats/{CinemaHallId}")]
        public IHttpActionResult GetSeats(int CinemaHallId)
        {
            DateTime sevenDays = DateTime.Now.AddDays(-7);

            foreach (Projections p in context.Projections.Where(x => x.CinemaHallID == CinemaHallId).ToList())
            {
                if (p.DateTimeStart > sevenDays)
                {
                  return Content(HttpStatusCode.BadRequest, new { start = p.DateTimeStart, movieName = p.Movies.Name });
                }
            }


            Dictionary<int, List<int>> seats = new Dictionary<int, List<int>>();

            if (seats == null)
            {
                return NotFound();
            }

            foreach (int row in context.Seats.Where(x => x.CinemaHallID == CinemaHallId && !x.IsDeleted).Select(x=> x.SeatRowID).Distinct().ToList())
            {
                seats.Add(row, context.Seats.Where(x => x.CinemaHallID == CinemaHallId && x.SeatRowID == row && !x.IsDeleted).Select(x => x.SeatColumnID).ToList());             
            }


            return Ok( new
            {
                SeatDict = seats,
            });
        }

        [HttpPost]
        [Route("GetCinemaHalls")]
        public IHttpActionResult GetCinemaHalls(dynamic Times)
        {

            int CinemaId = Times.cinemaId;

            DateTime startTime = Times.start;
            DateTime endTime = Times.end;
            DateTime sevenDays = DateTime.Now.AddDays(-7);
            List<CinemaHalls> ch = context.CinemaHalls.Where(x=> x.CinemaID == CinemaId && !x.IsDeleted).ToList();
            var p = context.Projections.Where(x => x.DateTimeStart > sevenDays && x.CinemaHalls.CinemaID == CinemaId).ToList();

            //POGLEDATI!!

            foreach (Projections c in p)
            {
                if (c.DateTimeStart == startTime)
                    ch.RemoveAll(x => x.CinemaHallID == c.CinemaHallID);
                else if (startTime > c.DateTimeStart && startTime <= c.DateTimeStart.AddMinutes(c.Movies.DurationInMinutes))
                    ch.RemoveAll(x => x.CinemaHallID == c.CinemaHallID);
                else if (startTime > c.DateTimeStart.AddMinutes(c.Movies.DurationInMinutes))
                {
                    foreach (Projections cc in context.Projections.Where(x => x.DateTimeStart > startTime && x.CinemaHalls.CinemaID == CinemaId))
                    {
                        if (cc.DateTimeStart <= endTime)
                            ch.RemoveAll(x => x.CinemaHallID == cc.CinemaHallID);
                    }
                }
            }

            if (Times.ProjectionId != null)
            { 
            int pId = Times.ProjectionId;
            ch.Add(context.Projections.Where(x => x.ProjectionID == pId).FirstOrDefault().CinemaHalls);
            }

            return Ok(ch.Select(x => new
            {
                Id = x.CinemaHallID,
                Name = x.Name,
            }));
        }
        
        [Route("GetMovies")]
        public IHttpActionResult GetMovies()
        {
            List<Movies> g = context.Movies.ToList();
            if (g == null)
            {
                return NotFound();
            }
            return Ok(g.Select(x => new
            {
                Id = x.MovieID,
                MovieName = x.Name,

            }));
        }

        [HttpGet]
        [Route("GetMovie/{MovieId}")]
        public IHttpActionResult GetMovie(int MovieId)
        {
            Movies g = context.Movies.Where(x => x.MovieID == MovieId).FirstOrDefault();
            if (g == null)
                return NotFound();

            Dictionary<int, int> actors = new Dictionary<int, int>();
            List<MovieRoles> roles = context.MovieRoles.Where(x => x.MovieID == MovieId).ToList();

            foreach (MovieRoles mr in roles)
            {
                actors.Add(mr.ActorID, mr.ActorID);
            }

            return Ok(new {
                Id = g.MovieID,
                MovieName = g.Name,
                originalName = g.OriginalName,
                Duration = g.DurationInMinutes,
                ReleaseYear = g.ReleaseYear,
                Synopsis = g.Synopsis,
                DirectorId = g.MovieDirectorID,
                GenreName = g.Genres.Name,
                Current = g.IsCurrent,
                Actors = actors,
                Director = g.MovieDirectorID,
                Age = g.AgeRestrictions.Name,
                Image = g.PictureBytes != null ? Convert.ToBase64String(g.PictureBytes) : null,
                Thumb = g.ImageThumb != null ? Convert.ToBase64String(g.ImageThumb) : null
            });
        }

        [HttpPost]
        [Route("PostMovies")]
        public IHttpActionResult addMovie(dynamic m)
        {
            Movies movie = new Movies();

            movie.AgeRestrictionID = m.movie.AgeRestrictionID;
            movie.GenreID = m.movie.GenreID;
            movie.Name = m.movie.Name;
            movie.OriginalName = m.movie.OriginalName;
            movie.ReleaseYear = m.movie.ReleaseYear;
            movie.PictureBytes = m.movie.PictureBytes;
            movie.Synopsis = m.movie.Synopsis;
            movie.DurationInMinutes = m.movie.DurationInMinutes;
            movie.Trailer = m.movie.Trailer;
            movie.ImageThumb = m.movie.ImageThumb;
            movie.IsCurrent = m.movie.IsCurrent;
            movie.IsDeleted = false;
            movie.MovieDirectorID = m.movie.MovieDirectorID;

            context.Movies.Add(movie);
            context.SaveChanges();



            foreach (int id in m.movieActor)
            {
                MovieRoles mr = new MovieRoles();
                mr.MovieID = movie.MovieID;
                mr.ActorID = id;

                context.MovieRoles.Add(mr);
                context.SaveChanges();
            }

            return Ok(new { MovieId = movie.MovieID });
        }


        [HttpPut]
        [Route("EditMovies")]
        public IHttpActionResult EditMovies(dynamic m)
        {
            int movieId = int.Parse(m.movie.MovieID.ToString());
            Movies movie = context.Movies.Where(x => x.MovieID == movieId).FirstOrDefault();

            movie.AgeRestrictionID = m.movie.AgeRestrictionID;
            movie.GenreID = m.movie.GenreID;
            movie.Name = m.movie.Name;
            movie.OriginalName = m.movie.OriginalName;
            movie.ReleaseYear = m.movie.ReleaseYear;
            movie.PictureBytes = m.movie.PictureBytes;
            movie.Synopsis = m.movie.Synopsis;
            movie.DurationInMinutes = m.movie.DurationInMinutes;
            movie.Trailer = m.movie.Trailer;
            movie.ImageThumb = m.movie.ImageThumb;
            movie.IsCurrent = m.movie.IsCurrent;
            movie.IsDeleted = false;
            movie.MovieDirectorID = m.movie.MovieDirectorID;
            context.SaveChanges();

            //List<Actors> actors = JsonConvert.DeserializeObject<List<Actors>>(m.movieActor.ToString());

            foreach (MovieRoles mr in context.MovieRoles.Where(x => x.MovieID == movieId).ToList())
            {
                context.MovieRoles.Remove(mr);
                context.SaveChanges();
            }


            foreach (int id in m.movieActor)
            {
                MovieRoles mr = new MovieRoles();
                mr.MovieID = movie.MovieID;
                mr.ActorID = id;
                mr.IsDeleted = false;

                context.MovieRoles.Add(mr);
                context.SaveChanges();
            }

            return Ok(new { MovieId = movie.MovieID });
        }
        [HttpGet]
        [Route("GetProjections/{cinemaId}")]
        public IHttpActionResult GetProjections(int cinemaId)
        {          
            List<Projections> g = context.Projections.Take(9).Where(x=> !x.IsDeleted && x.CinemaHalls.CinemaID == cinemaId).OrderByDescending(x => x.ProjectionID).ToList();
            if (g == null)
            {
                return NotFound();
            }
            return Ok(g.Select(x => new 
            {
                ProjectionId = x.ProjectionID,
                MovieId = x.MovieID,
                MovieName = x.Movies.Name,
                TechnologyTypeId = x.TechnologyTypeID,
                CinemaHallId = x.CinemaHallID,
                CinemaHall = x.CinemaHalls.Name,
                DateTimeStart = x.DateTimeStart,
                techTypeName = x.TechnologyTypes.Name,
                MovieImage = x.Movies.ImageThumb,
            }));
        }
        [Route("GetProjection/{ProjectionId}")]
        public IHttpActionResult GetProjection(int ProjectionId)
        {
            Projections x = context.Projections.Find(ProjectionId);
            if (x == null)
            {
                return NotFound();
            }
            return Ok(new
            {
                ProjectionId = x.ProjectionID,
                MovieId = x.MovieID,
                MovieName = x.Movies.Name,
                TechnologyTypeId = x.TechnologyTypeID,
                CinemaHallId = x.CinemaHallID,
                CinemaHall = x.CinemaHalls.Name,
                DateTimeStart = x.DateTimeStart,
                MovieImage = x.Movies.ImageThumb,
                techTypeName = x.TechnologyTypes.Name,
                price = x.TicketPrice
            });
        }


        [HttpPost]
        [Route("AddProjection")]
        public IHttpActionResult AddProjection(dynamic projection)
        {
            decimal price = projection.Price;

            Projections p = new Projections();
            p.MovieID = projection.MovieId;
            p.CinemaHallID = projection.CinemaHallId;
            p.DateTimeStart = projection.DateTimeStart;
            p.TechnologyTypeID = projection.techTypeId;
            p.TicketPrice =price;
            p.IsDeleted = false;

            context.Projections.Add(p);
            context.SaveChanges();


            return Ok(new { projectionId = p.ProjectionID});
        }

        [HttpPut]
        [Route("EditProjection")]
        public IHttpActionResult EditProjection(dynamic projection)
        {
            int projectionId = projection.ProjectionId;
            decimal price = projection.Price;

            Projections p = context.Projections.Where(x => x.ProjectionID == projectionId && !x.IsDeleted).FirstOrDefault();
            p.MovieID = projection.MovieId;
            p.CinemaHallID = projection.CinemaHallId;
            p.DateTimeStart = projection.DateTimeStart;
            p.TechnologyTypeID = projection.techTypeId;
            p.TicketPrice = price;
            p.IsDeleted = false;
            context.SaveChanges();


            return Ok(new { projectionId = p.ProjectionID });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }

    public class Dates
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
      

    }
}
