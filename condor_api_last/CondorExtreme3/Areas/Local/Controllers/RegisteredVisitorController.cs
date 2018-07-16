using System;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using CondorExtreme3.Areas.Local.Models;
using CondorExtreme3.DAL;
using CondorExtreme3.Tools;

/*-----------------------------------+
 * Author: Ilhan Karic               |
 * Module: Users                     |
 * Last Modified: 1/10/2018 @ 19:36  |
 * Copyright: Condor Coorporation    |
 *-----------------------------------*/
namespace CondorExtreme3.Areas.Local.Controllers
{
    public class RegisteredVisitorController : Controller
    {
        // Global user context (userDB)
        private CondorDBUsers contextGlobal = new CondorDBUsers();
        // Random number generator
        private Random rng = new Random();

        /// <summary>
        ///     RegisteredVisitor index page
        /// </summary>
        /// <param name="seats">String in format "ProjectionID, Seat1, Seat2, Seatn"</param>
        /// <param name="ProjectionId">ID of the projection</param>
        /// <returns></returns>
        public ActionResult Index(string seats = "", string ProjectionId = "")
        {
            // In case we are redirected from main page and need to make a reservation
            if (seats != "" && ProjectionId != "")
            {
                // At this point ProjectionId is redundant as you get it within seats
                // Fix this at a later time

                // Get all seats
                List<string> AllSeats = seats.Trim(',').Split(',').ToList();
                ModelsUser.RegisteredVisitor visitor = contextGlobal.RegisteredVisitors.Find(int.Parse(Session["UserID"].ToString()));

                // Use local context (this cinema instance)
                using (CondorDBContextChild contextLocal = new CondorDBContextChild(HttpContext.Session["ConnectionString"].ToString()))
                {
                    // Check if local cinema has this users country/city
                    if (!contextLocal.Cities.Any(x => x.Name == visitor.City.Name))
                    {
                        // If not make the country first
                        ModelsLocalDB.Country co = new ModelsLocalDB.Country();

                        // If the country doesn't exist
                        if (!contextLocal.Country.Any(x => x.Name == visitor.City.Country.Name))
                        {
                            // Add it to the local database
                            co = new ModelsLocalDB.Country()
                            {
                                Name = visitor.City.Country.Name,
                                IsDeleted = false
                            };

                            contextLocal.Country.Add(co);
                            contextLocal.SaveChanges();
                        }

                        // Repeat the same for the user city
                        ModelsLocalDB.City ci = new ModelsLocalDB.City()
                        {
                            CityID = visitor.CityID,
                            Name = visitor.City.Name,
                            PostalCode = visitor.City.PostalCode,
                            IsDeleted = false,
                            CountryID = co.CountryID,
                            Country = co
                        };

                        contextLocal.Cities.Add(ci);
                        contextLocal.SaveChanges();
                    }

                    // Check if this user exists in the local cinema (cross DB hence GUID)
                    // The user exists only if he reserved in this cinema previously
                    if (!contextLocal.RegisteredVisitors.Any(x => x.Guid == visitor.Guid))
                    {
                        // If the user doesn't exist copy him over with basic information
                        ModelsLocalDB.RegisteredVisitor localRV = new ModelsLocalDB.RegisteredVisitor();

                        // Do not copy username or password here for security reasons
                        localRV.RegisteredVisitorID = int.Parse(Session["UserID"].ToString());
                        localRV.FirstName = visitor.FirstName;
                        localRV.LastName = visitor.LastName;
                        localRV.PhoneNumber = visitor.PhoneNumber;
                        localRV.Email = visitor.Email;
                        // Make sure to copy over the GUID for the next search
                        localRV.Guid = visitor.Guid;  
                        localRV.CityID = contextLocal.Cities
                            .Any(x => x.Name == visitor.City.Name) ?
                            contextLocal.Cities.Where(x => x.Name == visitor.City.Name)
                            .SingleOrDefault().CityID : visitor.CityID;

                        contextLocal.RegisteredVisitors.Add(localRV);
                        contextLocal.SaveChanges();
                    }

                    // Create a new reservation (locally in the cinema)
                    ModelsLocalDB.Reservation r = new ModelsLocalDB.Reservation();
                    // Create a mirror on the (globally in the users database)
                    ModelsUser.Reservation r_mirror = new ModelsUser.Reservation();

                    r.RegisteredVisitorID = visitor.RegisteredVisitorID;
                    r.ProjectionID = int.Parse(ProjectionId);
                    r.PaymentMethod = contextLocal.PaymentMethods.FirstOrDefault();
                    r.IsDeleted = false;
                    r.ReservationDate = contextLocal.Projections.Find(r.ProjectionID).DateTimeStart;
                    r.ExpiryDate = contextLocal.Projections.Find(r.ProjectionID).DateTimeStart.Subtract(new TimeSpan(0, 30, 0));
                    r.ReservationCompleted = false;
                    // Create a new GUID
                    r.Guid = Guid.NewGuid().ToString();
                    
                    // Copy the basic information to the mirror
                    r_mirror.ConnString = HttpContext.Session["ConnectionString"].ToString();
                    r_mirror.RegisteredVisitor = visitor;
                    r_mirror.RegisteredVisitorID = visitor.RegisteredVisitorID;
                    // Copy the GUID of the previously created reservation
                    r_mirror.Guid = r.Guid;
                    
                    contextLocal.Reservations.Add(r);
                    contextLocal.SaveChanges();

                    contextGlobal.Reservations.Add(r_mirror);
                    contextGlobal.SaveChanges();

                    // Foreach seat (skip the first parameter ProjectID)
                    foreach (string seat in AllSeats.Skip(1))
                    {
                        // Find the projection
                        var projectionSeat = contextLocal.ProjectionSeats
                            .Find(int.Parse(ProjectionId), int.Parse(seat));

                        projectionSeat.IsReserved = true;
                        // Create the ticket for the seat
                        ModelsLocalDB.Ticket ticket = new ModelsLocalDB.Ticket();

                        ticket.SeatID = projectionSeat.SeatID;
                        ticket.TicketPrice = projectionSeat.Projection.TicketPrice;
                        ticket.TotalTicketPrice = ticket.TicketPrice;
                        ticket.TotalDiscountAmount = 0;
                        ticket.IsSold = false;
                        ticket.IsDeleted = false;
                        ticket.ReservationID = r.ReservationID;

                        contextLocal.Tickets.Add(ticket);
                    }

                    contextLocal.SaveChanges();
                }

                // Redirect to login if user is not logged
                if (Session["UserID"] == null)
                    return RedirectToAction("Login");
            }

            return View();
        }

        /// <summary>
        ///     User registration process.
        /// </summary>
        /// <returns>ActionResult view to the registration form.</returns>
        [HttpGet]
        public ActionResult Registration()
        {
            RegisteredVisitorVM model = new RegisteredVisitorVM();
            return View("Registration", model);
        }

        /// <summary>
        ///     Submits the registered visitor.
        /// </summary>
        /// <param name="model">The RegisteredVisitorVM model</param>
        /// <returns>ActionResult view to login page</returns>
        [HttpPost]
        public ActionResult SubmitRegisteredVisitors(RegisteredVisitorVM model)
        {
            // Return and raise ValidationErrorMessages if constraints not met
            if (!ModelState.IsValid)
                return View("Registration", model);

            // If the data is usable create a new registered visitor
            ModelsUser.RegisteredVisitor rv = new ModelsUser.RegisteredVisitor();
            // The salt is a random integer stored as string
            string salt = rng.Next().ToString();

            rv.FirstName = model.FirstName;
            rv.LastName = model.LastName;
            rv.Username = model.UserName;
            // SHA256 hash algorithm used for hashing the password + salt
            // The password and salt are both stored in the same column
            // They are separated by the '\0' character
            rv.Password = $"{Algorithm.GetStringSha256Hash(model.Password + salt)}\0{salt}";
            rv.Email = model.Email;
            rv.City = contextGlobal.Cities.Find(model.CityID);
            rv.City.Country = contextGlobal.Country.Find(rv.City.Country.CountryID);
            rv.CityID = rv.City.CityID;
            rv.VirtualPointsTotal = 0;
            rv.Guid = Guid.NewGuid().ToString();
            rv.PhoneNumber = model.PhoneNumber;

            contextGlobal.RegisteredVisitors.Add(rv);
            contextGlobal.SaveChanges();

            return RedirectToAction("Login");
        }

        /// <summary>
        ///     Logs the user into the session.
        /// </summary>
        /// <param name="model">The LoginVM model</param>
        /// <returns>ActionResult view to the login form</returns>
        [HttpGet]
        public ActionResult Login(LoginVM model = null)
        {
            if (Session["UserID"] != null)
                return RedirectToAction("Index");

            if (model == null)
                model = new LoginVM();

            ModelState.Clear();
            return View("Login", model);
        }

        /// <summary>
        ///     Logs the user out of session.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Logout()
        {
            Session["UserID"] = null;
            return RedirectToAction("Login");
        }

        /// <summary>
        ///     The actial username/password validation
        /// </summary>
        /// <param name="model">The LoginVM model</param>
        /// <returns>ActionResult view to Index or Login</returns>
        [HttpPost]
        public ActionResult LoginRequest(LoginVM model)
        {
            // If the user doesn't exist
            if (!contextGlobal.RegisteredVisitors.Any(x => x.Username == model.UserName))
            {
                model.ErrorMessage = "Invalid Username!";
                model.Password = "";
                return RedirectToAction("Login", model);
            }
            else
            {
                // Get the user password hash
                string pass = contextGlobal.RegisteredVisitors
                    .Where(x => x.Username == model.UserName)
                    .Select(x => x.Password).SingleOrDefault();

                // Separate the hash from the salt with split by the separator used '\0'
                string hash = pass.Split('\0')[0];
                string salt = pass.Split('\0')[1];

                // Use the same hashing algorithm (SHA256) to hash the entered password + salt
                string modelPass = Algorithm.GetStringSha256Hash(model.Password + salt);
                // If the hashes match
                if (modelPass == hash)
                {
                    Session["UserID"] = contextGlobal.RegisteredVisitors
                        .Where(x => x.Username == model.UserName)
                        .Select(x => x.RegisteredVisitorID).SingleOrDefault();

                    return RedirectToAction("Index");
                }
                else
                {
                    model.ErrorMessage = "Invalid Password!";
                    return RedirectToAction("Login", model);
                }
            }
        }
    }
}