using CondorExtreme3_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace CondorExtreme3_API.Controllers
{
    [RoutePrefix("api/CinemaManagement")]
    public class CinemaManagementController : ApiController
    {
        public CondorDBXEntities principal = new CondorDBXEntities();

        [HttpPost]
        [Route("SearchCinemaHalls")]
        public IHttpActionResult SearchCinemaHalls(dynamic queryObject)
        {
            int CinemaID = queryObject.CinemaID;
            string Name = queryObject.Name;
            var CinemaHalls = principal.CinemaHalls
                .Where(x => !x.IsDeleted 
                            && x.CinemaID == CinemaID 
                            && x.Name.Contains(Name)).ToList();
            return Ok(CinemaHalls.Select(x => new
            {
                CinemaHallID=x.CinemaHallID,
                Name=x.Name,
                NumberOfProjections=x.Projections.ToList().Count              
            }).ToList());
        }
        [HttpPost]
        [Route("PostCinemaHalls")]
        public IHttpActionResult PostCinemaHalls(dynamic CinemaHall)
        {
            int CinemaID = CinemaHall.CinemaID;
            string Name = CinemaHall.Name;
            int NumberOfSeatRows = CinemaHall.NumberOfSeatRows;
            int NumberOfSeatColumns = CinemaHall.NumberOfSeatColumns;

            CinemaHalls cinemaHall = new CinemaHalls();
            cinemaHall.CinemaID = CinemaID;
            cinemaHall.Name = Name;
            cinemaHall.IsDeleted = false;

            principal.CinemaHalls.Add(cinemaHall);
            principal.SaveChanges();

            Task.Run(() => SeatArrayHandler.InsertSeats(cinemaHall.CinemaHallID, NumberOfSeatRows, NumberOfSeatColumns));          
            return Ok();
        }
        [HttpPut]
        [Route("PutCinemaHalls")]
        public IHttpActionResult PutCinemaHalls(dynamic CinemaHall)
        {
            int CinemaHallID = CinemaHall.CinemaHallID;
            string Name = CinemaHall.Name;

            var ch = principal.CinemaHalls.Where(x => x.CinemaHallID == CinemaHallID).FirstOrDefault();
            ch.Name = Name;   
            principal.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("RemoveCinemaHalls")]
        public IHttpActionResult RemoveCinemaHalls(dynamic CinemaHall)
        {
            int CinemaHallID = CinemaHall.CinemaHallID;
            var ch = principal.CinemaHalls.Where(x => x.CinemaHallID == CinemaHallID).FirstOrDefault();
            ch.IsDeleted = true;
            principal.SaveChanges();
            return Ok();
        }

    }

    public class SeatArrayHandler
    {
        static CondorDBXEntities principal = new CondorDBXEntities();

        public static void InsertSeats(int CinemaHallID, int NumberOfSeatRows, int NumberOfSeatColumns)
        {
            List<Seats> allSeats = (from SR in principal.SeatRows.Take(NumberOfSeatRows)
                                    from SC in principal.SeatColumns.Take(NumberOfSeatColumns)
                                    select new
                                    {
                                        SR.SeatRowID,
                                        SC.SeatColumnID
                                    }).ToList().Select(x => new Seats
                                    {
                                        CinemaHallID = CinemaHallID,
                                        SeatRowID = x.SeatRowID,
                                        SeatColumnID = x.SeatColumnID,
                                        IsDeleted = false
                                    }).ToList();

            principal.Seats.AddRange(allSeats);
            principal.SaveChanges();
        }
    }
}
