using CondorExtreme3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CondorExtreme3.Areas.Local.Models
{
    public class EditEmployeeVM
    {
        public int EmployeeID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int CityID { get; set; }

        public List<SelectListItem> ListCities { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string BirthDate { get; set; }

        public bool Gender { get; set; }

        public string HireDate { get; set; }

        public double Salary { get; set; }

        public string Username { get; set; }


        public string Password { get; set; }


        public bool ProjectionManager { get;  set; }

        public bool Employee { get; set; }


        public string SelectedGender { get; set; }

        public List<Gender> Genders { get; set; }


    }
}