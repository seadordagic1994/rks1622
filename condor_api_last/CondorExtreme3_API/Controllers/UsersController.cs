using System;
using System.Collections.Generic;
using CondorExtreme3_API.Models;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Web.Http.Results;
using Newtonsoft.Json.Linq;


namespace CondorExtreme3_API.Controllers
{
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private CondorDBXEntities principal = new CondorDBXEntities();

        // GET api/Users
        [HttpGet]
        [Route("GetUsers")]
        public IHttpActionResult GetUsers()
        {
            var users = principal.RVisitors.Where(x => !x.IsDeleted).ToList();

            if (!users.Any())
                return BadRequest("There are no users registered currently!");

            return Ok(users.Select(x => new
            {
                x.RVisitorID,
                x.FirstName,
                x.LastName,
                x.Email,
                x.PhoneNumber,
                x.VirtualPointsTotal,
                City = x.Cities.Name
            }).ToList());
        }

        // GET api/Users
        [HttpGet]
        [Route("GetUsers/{RVisitorID}")]
        public IHttpActionResult GetUser([FromUri]int RVisitorID)
        {
            var user = principal.RVisitors.Find(RVisitorID);

            if (user == null)
                return BadRequest("There user doesn't exist!");

            return Ok(new {
                user.RVisitorID,
                user.FirstName,
                user.LastName,
                user.Email,
                user.PhoneNumber,
                user.VirtualPointsTotal,
                user.Cities.Name
            });
        }

        [HttpPost]
        [Route("PostUsersLogin")]
        public IHttpActionResult PostUsersLogin([FromBody]RVisitors value)
        {
            // Because users aren't unique for now
            var possibleUsers = principal.RVisitors
                 .Where(x => !x.IsDeleted && x.Username == value.Username);

            foreach (var user in possibleUsers)
            {
                var passHash = CondorExtreme3.Tools.Algorithm.GetStringSha256Hash(value.PasswordHash + user.PasswordSalt);
                if (passHash == user.PasswordHash)
                    return Ok(new JObject {
                        { "UserID", user.RVisitorID },
                        { "FirstName", user.FirstName },
                        { "LastName", user.LastName },
                        { "Email", user.Email },
                        { "VirtualPoints", user.VirtualPointsTotal },
                        { "PhoneNumber", user.PhoneNumber }
                    });
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("PostUsers")]
        // POST api/<controller>
        public IHttpActionResult PostUsers([FromBody]RVisitors value)
        {
            JObject modelErrors = new JObject
            {
                { "FirstName", "" },
                { "LastName", "" },
                { "Email", "" },
                { "PhoneNumber", "" },
                { "City", "" },
                { "Username", "" },
                { "Password", "" }
            };

            if (principal.RVisitors.Where(x => x.Email == value.Email).FirstOrDefault() != null)
            {
                return BadRequest("User with that email already exists !");
            }

            if (principal.RVisitors.Where(x => x.PhoneNumber == value.PhoneNumber).FirstOrDefault() != null)
            {
                return BadRequest("User with that phone number already exists !");
            }

            var salt = new Random().Next().ToString();
            var saltedPass = CondorExtreme3.Tools.Algorithm.GetStringSha256Hash(value.PasswordHash + salt);

            value.PasswordHash = saltedPass;
            value.PasswordSalt = salt;

            principal.RVisitors.Add(value);
            principal.SaveChanges();
            
            return CreatedAtRoute("DefaultApi", new { id = value.RVisitorID }, value.RVisitorID);
        }

    [HttpPost]
    [Route("PostUsersForAndroid")]
    // POST api/<controller>
    public IHttpActionResult PostUsersForAndroid([FromBody]RVisitors value)
    {
        JObject modelErrors = new JObject
            {
                { "FirstName", "" },
                { "LastName", "" },
                { "Email", "" },
                { "PhoneNumber", "" },
                { "City", "" },
                { "Username", "" },
                { "Password", "" }
            };

        bool modelStateValid = true;

        if (value.FirstName == null)
        {
            modelErrors["FirstName"] = "This field is required!";
            modelStateValid = false;
        }

        if (value.LastName == null)
        {
            modelErrors["LastName"] = "This field is required!";
            modelStateValid = false;
        }

        if (value.Email == null)
        {
            modelErrors["Email"] = "This field is required!";
            modelStateValid = false;
        }
        else
        if (!System.Text.RegularExpressions.Regex.IsMatch(value.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
        {
            modelErrors["Email"] = "Invalid format!";
            modelStateValid = false;
        }

        if (value.PhoneNumber == null)
        {
            modelErrors["PhoneNumber"] = "This field is required!";
            modelStateValid = false;
        }

        if (value.CityID == null || value.CityID < 0)
        {
            modelErrors["City"] = "This field is required!";
            modelStateValid = false;
        }

        if (value.Username == null)
        {
            modelErrors["Username"] = "This field is required!";
            modelStateValid = false;
        }

        if (value.PasswordHash == null)
        {
            modelErrors["Password"] = "This field is required!";
            modelStateValid = false;
        }

        if (!modelStateValid)
            return Content(System.Net.HttpStatusCode.BadRequest, modelErrors);

        var salt = new Random().Next().ToString();
        var saltedPass = CondorExtreme3.Tools.Algorithm.GetStringSha256Hash(value.PasswordHash + salt);

        value.PasswordHash = saltedPass;
        value.PasswordSalt = salt;

        principal.RVisitors.Add(value);
        principal.SaveChanges();

        return Ok(value.RVisitorID);
    }
}
}   