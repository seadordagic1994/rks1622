using System;
using System.Web.Mvc;
using CondorExtreme3.Areas.Local.Models;
using CondorExtreme3.ModelsLocalDB;
using CondorExtreme3.DAL;
using CondorExtreme3.Tools;
using System.Linq;
using System.Collections.Generic;

/*-----------------------------------+
 * Author: Ilhan Karic               |
 * Module: Users                     |
 * Last Modified: 1/10/2018 @ 19:36  |
 * Copyright: Condor Coorporation    |
 *-----------------------------------*/
namespace CondorExtreme3.Areas.Local.Controllers
{
    public class VisitorController : Controller
    {
        // Random number generator
        private Random rng = new Random();

        // Singleton design pattern
        private CondorDBContextChild _contextLocal;
        public CondorDBContextChild contextLocal
        {
            get
            {
                if (HttpContext.Session["ConnectionString"].ToString() == "")
                { return _contextLocal; }
                if (_contextLocal == null)
                    _contextLocal = new CondorDBContextChild(HttpContext.Session["ConnectionString"].ToString());
                return _contextLocal;
            }
            set { _contextLocal = value; }
        }

        /// <summary>
        ///     Reserving as unregistered visitor.
        /// </summary>
        /// <param name="Seats">List of seats to reserve</param>
        /// <param name="model">The VisitorVM model</param>
        /// <returns>ActionResult view to registration</returns>
        [HttpPost]
        public ActionResult Registration(string[] Seats, VisitorVM model = null)
        {
            ModelState.Clear();
            List<string> GetSeats = Seats.ToList();
            string s = "";
            foreach (string seat in GetSeats)
                s += seat + ",";

            model.ProjectionId = GetSeats[0];
            GetSeats.RemoveAt(0);

            if (Session["UserID"] != null)
                return RedirectToAction("Index", "RegisteredVisitor", new { seats = s, ProjectionId = model.ProjectionId });

            if (contextLocal == null)
                return Redirect(Url.Content("~/"));

            if (model == null)
                model = new VisitorVM();

            model.Seats = GetSeats;

            return View("Registration", model);
        }

        /// <summary>
        ///     Submits the new unregistered visitor.
        /// </summary>
        /// <param name="model">The VisitorVM model</param>
        /// <param name="Seats">List of seats in format "Seat1, Seat2, Seatn"</param>
        /// <returns>ActionResult view to RegistrationActivation</returns>
        [HttpPost]
        public ActionResult SubmitVisitors(VisitorVM model, string Seats)
        {
            List<string> AllSeats = Seats.Trim(',').Split(',').ToList();

            if (contextLocal == null)
                return Redirect(Url.Content("~/"));

            if (!ModelState.IsValid)
                return View("Registration", model);

            // Create new activation code
            model.ActivationCode = rng.Next(1000, 10000).ToString();
            model.Seats = AllSeats;

            // Add the phone number and activation code to the current session
            Session["ActivationCode"] = model.ActivationCode;
            Session["PhoneNumber"] = model.PhoneNumber;

            // Send SMS with the activation code for phone number verification

            TwilioHelper.SendSMS("+" + model.PhoneNumber, model.ActivationCode);
            return View("RegistrationActivation", model);
        }

        /// <summary>
        ///     Confirm the visitors phone number.
        /// </summary>
        /// <param name="model">The VisitorVM model</param>
        /// <param name="Seats">List of seats in format "Seat1, Seat2, Seatn"</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConfirmVisitors(VisitorVM model, string Seats)
        {
            if (contextLocal == null)
                return Redirect(Url.Content("~/"));
            Visitor v;
            // Get the phone number entered previously from the session
            model.PhoneNumber = Session["PhoneNumber"].ToString();
            List<string> AllSeats = Seats.Trim(',').Split(',').ToList();
            // Check if the entered code (from SMS) matches the actual given code
            if (model.ActivationConfirmationCode == Session["ActivationCode"].ToString())
            {
                // Check if this phone number was registered before
                if (!contextLocal.Visitors.Any(x => x.PhoneNumber == model.PhoneNumber))
                {
                    // In the case it wasn't create a new user
                    v = new Visitor();
                    v.ActivationCode = Session["ActivationCode"].ToString();
                    v.PhoneNumber = Session["PhoneNumber"].ToString();
                    contextLocal.Visitors.Add(v);
                    contextLocal.SaveChanges();
                }
                else
                {
                    // In case it was just update with new data
                    v = contextLocal.Visitors.Where(x => x.PhoneNumber == model.PhoneNumber).FirstOrDefault();
                    v.ActivationCode = Session["ActivationCode"].ToString();
                    v.PhoneNumber = Session["PhoneNumber"].ToString();
                    contextLocal.SaveChanges();
                }

                int i = 0;
                bool seatsReserved = false;
                // Check if picked seats are available
                foreach (ProjectionSeat s in contextLocal.ProjectionSeats.Where(x=> x.ProjectionID.ToString() == model.ProjectionId).ToList())
                    if (s.SeatID.ToString() == AllSeats[i] && s.IsReserved)
                        seatsReserved = true;

                // If they are not reserved
                if (!seatsReserved)
                {
                    foreach (ProjectionSeat s in contextLocal.ProjectionSeats.Where(x => x.ProjectionID.ToString() == model.ProjectionId).ToList())
                    {
                        if (i < AllSeats.Count)
                        {
                            foreach (string seat in AllSeats)
                            {
                                // Reserve them
                                if (s.SeatID.ToString() == seat)
                                    s.IsReserved = true;
                            }
                        }
                            
                    }
                    
                    // Create a new reservation
                    Reservation r = new Reservation();

                    r.VisitorID = v.VisitorID;
                    r.Visitor = contextLocal.Visitors.Find(v.VisitorID);
                    r.PaymentMethod = contextLocal.PaymentMethods.FirstOrDefault();
                    r.Visitor = v;
                    r.ProjectionID = int.Parse(model.ProjectionId);
                    r.Projection = contextLocal.Projections.Where(x => x.ProjectionID == r.ProjectionID).FirstOrDefault();
                    // Used to check if the reservation was paid for later
                    r.ReservationCompleted = true;
                    r.ReservationDate = DateTime.Now;
                    r.ExpiryDate = r.Projection.DateTimeStart.Subtract(new TimeSpan(0, 30, 0));
                    // Used to check if canceled or not later
                    r.IsDeleted = false;

                    contextLocal.Reservations.Add(r);
                    contextLocal.SaveChanges();
                }
                else return Content("Somebody reserved the seats meanwhile!");

                Session["ActivationCode"] = null;
                Session["PhoneNumber"] = null;
            }
            else
            {
                Session["ActivationCode"] = null;
                Session["PhoneNumber"] = null;

                model.ErrorMessage = "Invalid Activation Code!";
                return RedirectToAction("Registration", model);
            }
            return RedirectToAction("Cinema", "../Local", new { Reservation  = "Your reservation has been made!"});
        }
    }
}
