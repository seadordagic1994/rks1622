using CondorExtreme3.ModelsUser;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using CondorExtreme3.DAL;
using CondorExtreme3.Tools;
using System.Web;

/*-----------------------------------+
 * Author: Ilhan Karic               |
 * Module: Users                     |
 * Last Modified: 1/10/2018 @ 19:36  |
 * Copyright: Condor Coorporation    |
 *-----------------------------------*/
namespace CondorExtreme3.Areas.Local.Models
{
    public class RegisteredVisitorVM
    {
        [MaxLength(50, ErrorMessage = "First name is too long!")]
        [Required(ErrorMessage = "First name is required!")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "Last name is too long!")]
        [Required(ErrorMessage = "Last name is required!")]
        public string LastName { get; set; }
        
        [MaxLength(30, ErrorMessage = "Username is too long!")]
        [Required(ErrorMessage = "Username is required!")]
        //[Unique("Username already exists!", typeof(RegisteredVisitor))] [DISABLED]
        public string UserName { get; set; }

        [MaxLength(30, ErrorMessage = "Password is too long!")]
        [MinLength(8, ErrorMessage = "Must have at least 8 characters!")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,15}$", ErrorMessage = "Must be alpha-numeric!")]
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone number is required!")]
        [MaxLength(50, ErrorMessage = "Phone number too long!")]
        //[Unique("Number already exists!", typeof(Visitor))] [DISABLED]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Email is required!")]
        [RegularExpression("^[^@]+@[^@]+\\.[^@]+$", ErrorMessage = "Invalid format!")]
        //[Unique("Email already exists!", typeof(RegisteredVisitor))] [DISABLED]
        public string Email { get; set; }

        [Required(ErrorMessage = "City is required!")]
        public int CityID { get; set; }
        public List<SelectListItem> SelectListCities { get; set; } = new List<SelectListItem>();

        public RegisteredVisitorVM()
        {
            CondorDBUsers principal = new CondorDBUsers();
            List<City> cities = principal.Cities.ToList().OrderBy(x => x.Name).ToList();

            SelectListCities.AddRange(cities.Select(
                    x => new SelectListItem { Value = x.CityID.ToString(), Text = x.Name }
                ).ToList()
            );
        }
    }
}