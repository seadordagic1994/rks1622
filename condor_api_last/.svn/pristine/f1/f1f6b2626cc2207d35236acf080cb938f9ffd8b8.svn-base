using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CondorExtreme3.DAL;
using CondorExtreme3.Helper;

/*-----------------------------------+
 * Author: Ilhan Karic               |
 * Module: Users                     |
 * Last Modified: 1/10/2018 @ 19:36  |
 * Copyright: Condor Coorporation    |
 *-----------------------------------*/
namespace CondorExtreme3.Areas.Local.Controllers
{
    public class ReservationController : Controller
    {
        public CondorDBUsers contextGlobal = new CondorDBUsers();

        /// <summary>
        ///     Runs through all user reservations (global level)
        ///     and finds the corresponding local reservations (cinema level).
        /// </summary>
        /// <returns>List of (local level) reservations.</returns>
        private List<Models.ReservationVM> GetForeignReservations()
        {
            // Find all global (user) reservations
            var globalReservations = contextGlobal.Reservations
                .Where(x => x.ConnString != null)
                .ToList();

            // Create a list of foreign reservations
            var foreignReservations = new List<Models.ReservationVM>();
            foreach (var reservation in globalReservations)
            {
                var tempConnection = new CondorDBContextChild(reservation.ConnString);
                // Where the GUID of the reservation matches the global GUID
                var foreignReservation = tempConnection.Reservations
                        .Where(x => x.Guid == reservation.Guid)
                        .FirstOrDefault();

                if (foreignReservation != null)
                {
                    var newReservation = new Models.ReservationVM
                    {
                        ReservationID = foreignReservation.ReservationID,
                        ReservationDate = foreignReservation.ReservationDate,
                        ExpiryDate = foreignReservation.ExpiryDate,
                        Status = foreignReservation.ReservationCompleted ?
                            "Approved" : foreignReservation.IsDeleted ?
                            "Canceled" : "Pending",
                        Discount = foreignReservation.Tickets.Sum(x => x.TotalDiscountAmount),
                        Total = foreignReservation.Tickets.Sum(x => x.TotalTicketPrice),
                        ConnectionString = reservation.ConnString
                    };

                    foreignReservations.Add(newReservation);
                }
            }

            return foreignReservations;
        }

        /// <summary>
        ///     Finds all foreign reservations and returns to partial view.
        /// </summary>
        /// <returns>List of foreign reservations.</returns>
        [HttpPost]
        public PartialViewResult GetReservations()
        {
            // ModelsUser.RegisteredVisitor [global for all cinemas]
            var globalUser = contextGlobal.RegisteredVisitors.Find(Session["UserID"]);

            // Find all global mirror reservations where a conn string exists
            var globalReservations = contextGlobal.Reservations
                .Where(x => x.ConnString != null)
                .ToList();

            // Foreach global reservation find its foreign local reservation
            var foreignReservations = GetForeignReservations();

            return PartialView("GetReservations", foreignReservations);
        }


        /// <summary>
        ///     Marks the given local reservation as canceled.
        /// </summary>
        /// <param name="reservationID">The ID of the reservation</param>
        /// <param name="connectionString">The connection string to the cinema</param>
        /// <returns>ActionResult to RV index page</returns>
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Cancel")]
        public ActionResult CancelReservation(int reservationID, string connectionString)
        {
            // Find the desired local reservation
            CondorDBContextChild localContext = new CondorDBContextChild(connectionString);
            var reservation = localContext.Reservations.Find(reservationID);
            // Mark it as deleted (canceled)
            reservation.IsDeleted = true;
            localContext.SaveChanges();

            return RedirectToAction("Index", "RegisteredVisitor");
        }

        /// <summary>
        ///     Processes the payment and completes the reservation if possible.
        /// </summary>
        /// <param name="reservationID">The ID of the reservation</param>
        /// <param name="connectionString">The connection string to the cinema</param>
        /// <returns>ActionResult to RV index page</returns>
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Confirm")]
        public ActionResult ConfirmReservation(int reservationID, string connectionString)
        {
            // Find the desired local reservation
            CondorDBContextChild localContext = new CondorDBContextChild(connectionString);
            // Find the desired global user
            var globalUser = contextGlobal.RegisteredVisitors.Find(Session["UserID"]);
            var reservation = localContext.Reservations.Find(reservationID);
            // Calculate the price as the sum of all ticket prices in the reservation
            var reservationPrice = reservation.Tickets.Sum(x => x.TotalTicketPrice);

            // The user has enough to proceed
            if (globalUser.VirtualPointsTotal >= reservationPrice)
            {
                reservation.ReservationCompleted = true;
                globalUser.VirtualPointsTotal -= (int)reservationPrice;
                localContext.SaveChanges();
                contextGlobal.SaveChanges();
            }

            return RedirectToAction("Index", "RegisteredVisitor");
        }
    }
}