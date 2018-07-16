using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CondorExtreme3_API.Models;
using Newtonsoft.Json.Linq;

namespace CondorExtreme3_API.Controllers
{
    [RoutePrefix("api/UserReservations")]
    public class UserReservationsController : ApiController
    {
        private CondorDBXEntities principal = new CondorDBXEntities();

        [HttpPost]
        [Route("PostMakeReservation")]

        public IHttpActionResult PostMakeReservation([FromBody]dynamic value)
        {
            Reservations reservation = new Reservations();

            try
            {
                JObject jObj = JObject.FromObject(value);

                reservation.InitialDate = DateTime.Now;
                reservation.ProjectionID = int.Parse(jObj["ProjectionID"].ToString());
                reservation.RVisitorID = int.Parse(jObj["RVisitorID"].ToString());

                var projection = principal.Projections.Find(reservation.ProjectionID);
                var movieDuration = principal.Movies.Find(projection.MovieID).DurationInMinutes;
                var endDate = projection.DateTimeStart.AddMinutes(movieDuration);

                reservation.ValidUntil = endDate;
                reservation.IsCompleted = 0; // <-- potential future error: change from int to bool

                reservation.PaymentMethodID = 2; // <-- virtual points, set manually for now

                reservation.PhoneNumberVis = principal.RVisitors.Find(int.Parse(jObj["RVisitorID"].ToString())).PhoneNumber;
                reservation.CreditCardNumber = null;

                principal.Reservations.Add(reservation);
                principal.SaveChanges();
                
                JArray jSeats = JArray.FromObject(jObj["SelectedSeats"]);

                foreach (var seatID in jSeats)
                {
                    var ticket = new Tickets()
                    {
                        ProjectionID = (int)reservation.ProjectionID,
                        ReservationID = reservation.ReservationID,
                        SeatID = seatID.Value<int>(),
                        TicketNumber = Guid.NewGuid().GetHashCode(),
                        TicketPrice = projection.TicketPrice
                    };
                    principal.Tickets.Add(ticket);
                }

                principal.SaveChanges();
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }

            return Ok($"Reservation completed! ## {reservation.ReservationID}");
        }

        [HttpGet]
        [Route("GetUserReservations/{userID}")]
        public IHttpActionResult GetUserReservations([FromUri]int userID)
        {
            var reservations = principal.Reservations.Where(x => x.RVisitorID == userID && x.IsCompleted == 0).ToList();

            return Ok(reservations.Select(x => new {
                x.ReservationID,
                MovieName = principal.Movies.Find(principal.Projections.Find(x.ProjectionID).MovieID).Name,
                OrderDate = x.InitialDate,
                ExpireDate = x.ValidUntil,
                Discount = 0,
                Total = principal.Tickets
                    .Where(y => y.ReservationID == x.ReservationID)
                    .Select(y => y.TicketPrice).ToList().Sum()
            }));
        }

        [HttpPost]
        [Route("PostConfirmReservation")]
        public IHttpActionResult PostConfirmReservation([FromBody]dynamic value)
        {
            var jObj = JObject.FromObject(value);
            RVisitors user = principal.RVisitors.Find(int.Parse(jObj["RVisitorID"].ToString()));
            Reservations reservation = principal.Reservations.Find(int.Parse(jObj["ReservationID"].ToString()));

            if (user == null) return BadRequest("User does not exist");
            if (reservation == null) return BadRequest("Reservation does not exist");

            var totalCost = principal.Tickets
                .Where(x => x.ReservationID == reservation.ReservationID)
                .Select(x => x.TicketPrice).ToList().Sum();

            if (user.VirtualPointsTotal < totalCost)
                return BadRequest("You do not have enough VP for this purchase!");
            else
            {
                reservation.IsCompleted = 1;
                // Implement this at a later time! (Requires DB re-design)
                //principal.TransactionsVirtualPoints.Add(new TransactionsVirtualPoints() {
                //    RVisitorID = user.RVisitorID,
                //    TransactionDate = DateTime.Now,
                //    TransactionValue = -totalCost
                //});
                user.VirtualPointsTotal -= (int)Math.Ceiling(totalCost); // VirtualPointsTotal must be double!
                principal.SaveChanges();
                return Ok("You have successfully confirmed your reservation!");
            }
        }

        [HttpPost]
        [Route("PostCancelReservation")]
        public IHttpActionResult PostCancelReservation([FromBody]dynamic value)
        {
            var jObj = JObject.FromObject(value);
            RVisitors user = principal.RVisitors.Find(int.Parse(jObj["RVisitorID"].ToString()));
            Reservations reservation = principal.Reservations.Find(int.Parse(jObj["ReservationID"].ToString()));

            if (user == null) return BadRequest("User does not exist!");
            if (reservation == null) return BadRequest("Reservation does not exist!");

            // Delete all tickets
            var ticketsToRemove = principal.Tickets.Where(x => x.ReservationID == reservation.ReservationID);
            foreach (var ticket in ticketsToRemove)
                principal.Tickets.Remove(ticket);

            // Remove the reservation
            principal.Reservations.Remove(reservation);
            principal.SaveChanges();
            return Ok("You have successfully canceled your reservation!");
        }
    }
}