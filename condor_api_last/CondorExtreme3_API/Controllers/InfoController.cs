using CondorExtreme3_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CondorExtreme3_API.Controllers
{
    [RoutePrefix("api/Info")]
    public class InfoController : ApiController
    {
        public CondorDBXEntities principal = new CondorDBXEntities();

        [Route("GetCities")]
        public IHttpActionResult GetCities()
        {
            List<Cities> Cities = principal.Cities.Where(x => x.IsDeleted == false).ToList();
            if (Cities == null)
                return NotFound();
            return Ok(Cities.Select(x => new
            {
                CityID = x.CityID,
                Name = x.Name,
                PostalCode = x.PostalCode
            }).ToList());

        }
        [Route("GetCitiesByCountry/{CountryID}")]
        public IHttpActionResult GetCitiesByCountry(int CountryID)
        {
            List<Cities> Cities = principal.Cities.Where(x => x.IsDeleted == false && x.CountryID == CountryID).ToList();
            if (Cities.Count==0)
                return NotFound();
            return Ok(Cities.Select(x => new
            {
                CityID = x.CityID,
                Name = x.Name,
                PostalCode = x.PostalCode
            }).ToList());

        }
        [Route("GetCountries")]
        public IHttpActionResult GetCountries()
        {
            List<Countries> Countries = principal.Countries.Where(x => x.IsDeleted == false).ToList();
            if (Countries == null)
                return NotFound();
            return Ok(Countries.Select(x => new
            {
                CountryID = x.CountryID,
                Name = x.Name
            }).ToList());
        }
        [Route("GetCinemas")]
        public IHttpActionResult GetCinemas()
        {
            List<Cinemas> cinemas = principal.Cinemas.Where(x => x.IsDeleted == false).ToList();

            if (cinemas.Count == 0)
                return NotFound();
            return Ok(cinemas.Select(x => new
            {
                CinemaID = x.CinemaID,
                Name = x.Name,
                Address = x.Address,
                CityID = x.CityID
            }).ToList());
        }
        [Route("GetCinemas/{CinemaID}")]
        public IHttpActionResult GetCinemas(int CinemaID)
        {
            List<Cinemas> Cinemas = principal.Cinemas.Where(x => x.IsDeleted == false && x.CinemaID == CinemaID).ToList();

            if (Cinemas.Count == 0)
                return NotFound();
            return Ok(Cinemas.Select(x => new
            {
                CinemaID = x.CinemaID,
                Name = x.Name,
                Address = x.Address,
                CityID = x.CityID
            }).ToList());
        }

        [Route("GetCinemasByCity/{CityID}")]
        public IHttpActionResult GetCinemasByCity(int CityID)
        {
            List<Cinemas> Cinemas = principal.Cinemas.Where(x => x.IsDeleted == false && x.CityID == CityID).ToList();

            if (Cinemas.Count == 0)
                return NotFound();
            return Ok(Cinemas.Select(x => new
            {
                CinemaID = x.CinemaID,
                Name = x.Name,
                Address = x.Address,
                CityID = x.CityID
            }).ToList());
        }
        [Route("GetRootUsers/{Username}")]
        public IHttpActionResult GetRootUsers(string Username)
        {
            RootUsers RU = principal.RootUsers.Where(x => x.Username == Username).FirstOrDefault();

            if (RU == null)
                return NotFound();
            return Ok(new {
                RootUserID=RU.RootUserID,
                Username=RU.Username,
                PasswordHash=RU.PasswordHash,
                PasswordSalt= RU.PasswordSalt
            });
        }

        [Route("GetMovies")]
        public IHttpActionResult GetMovies()
        {
            List<Movies> moviesList = principal.Movies.ToList();

            if (moviesList.Count ==0)
                return NotFound();
            return Ok(moviesList.Select(x=> new
            {
                MovieID=x.MovieID,
                Name=x.Name           
            }         
            ));
        }

        [Route("GetTechnologyTypes")]
        public IHttpActionResult GetTechnologyTypes()
        {
            List<TechnologyTypes> techtypesList = principal.TechnologyTypes.ToList();

            if (techtypesList.Count == 0)
                return NotFound();
            return Ok(techtypesList.Select(x => new
            {
                TechnologyTypeID = x.TechnologyTypeID,
                Name = x.Name
            }
            ));
        }

        [Route("GetGenres")]
        public IHttpActionResult GetGenres()
        {
            List<Genres> Genres = principal.Genres.ToList();

            if (Genres.Count == 0)
                return NotFound();
            return Ok(Genres.Select(x => new
            {
                GenreID = x.GenreID,
                Name = x.Name
            }
            ));
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (principal != null)
                {
                    principal.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
