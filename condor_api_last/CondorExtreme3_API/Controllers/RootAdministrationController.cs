using CondorExtreme3_API.Helper;
using CondorExtreme3_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static CondorExtreme3_API.Helper.FrameworkExceptionHandler;

namespace CondorExtreme3_API.Controllers
{
    [RoutePrefix("api/RootAdministration")]
    public class RootAdministrationController : ApiController
    {
        public CondorDBXEntities principal = new CondorDBXEntities();

        [HttpGet]
        [Route("GetCitiesWithCountryNames")]
        public IHttpActionResult GetCitiesWithCountryNames()
        {
            var listCities = principal.Cities.ToList();
            if (listCities.Count==0)
                return NotFound();
            var finalList = listCities.Select(x => new
            {
                CityID = x.CityID,
                CityNameAndCountry = x.Name + ", " + x.Countries.Name
            }).ToList();
            finalList.Insert(0, new
            {
                CityID = 0,
                CityNameAndCountry = "(All Cities)"
            });
            return Ok(finalList);
        }

        [HttpPost]
        [Route("SearchCinemas")]
        public IHttpActionResult SearchCinemas(dynamic queryObject)
        {
            string CinemaName = queryObject.CinemaName;
            int CityID = queryObject.CityID;

            var CityList = (CityID == 0)
                ? principal.Cities.ToList().Select(x => x.CityID).ToList()
                : principal.Cities.ToList().Where(x => x.CityID == CityID).Select(x => x.CityID).ToList();

            var listCinemas = principal.Cinemas
                .Where(x => !x.IsDeleted
                        && CityList.Contains(x.CityID)
                        && x.Name.Contains(CinemaName)).ToList();
          
            return Ok(listCinemas.Select(x=>new CinemaInfo
            {
                CinemaID=x.CinemaID,
                Name=x.Name,
                Address=x.Address,
                CinemaHallTotal=x.CinemaHalls.ToList().Count,
                EmployeeTotal = x.Employees.ToList().Count
            }).ToList());
        }

        [HttpPost]
        [Route("PostCinemasAndDirectors")]
        public IHttpActionResult PostCinemasAndDirectors(CinemaDirectorComplex cinemaDirectorComplex)
        {
            var employeeInfo = cinemaDirectorComplex.employeeInfo;
            var cinemaInfo = cinemaDirectorComplex.cinemaInfo;

           
            var employeeSamePhoneNumber = principal.Employees.Where(x => x.PhoneNumber == employeeInfo.PhoneNumber && !x.IsDeleted).FirstOrDefault();
            if (employeeSamePhoneNumber != null)
                return Content(HttpStatusCode.BadRequest, ExceptionsMapper[ExceptionType.PhoneUniquenessViolation]);

            var employeeSameEmail = principal.Employees.Where(x => x.Email == employeeInfo.Email && !x.IsDeleted).FirstOrDefault();
            if (employeeSameEmail != null)
                return Content(HttpStatusCode.BadRequest, ExceptionsMapper[ExceptionType.EmailUniquenessViolation]);
                 

            //There is no need to check for usernames, since usernames are only restricted to one cinema.
            //Username uniqueness only exists on a level of a cinema
            //Since we are adding a new cinema and director, there are no employees which are related to it
            //So there is no need to check for username uniqueness


            Cinemas cinema = new Cinemas();
            cinema.Name = cinemaInfo.Name;
            cinema.CityID = cinemaInfo.CityID;
            cinema.Address = cinemaInfo.Address;
            cinema.IsDeleted = false;

            principal.Cinemas.Add(cinema);
            principal.SaveChanges();

            Employees EMP = new Employees();

            EMP.FirstName = employeeInfo.FirstName;
            EMP.LastName = employeeInfo.LastName;
            EMP.CityBirthID = employeeInfo.CityBirthID;
            EMP.BirthDate = employeeInfo.BirthDate;
            EMP.Gender = employeeInfo.Gender;
            EMP.CurriculumVitae = employeeInfo.CurriculumVitae;
            EMP.Email = employeeInfo.Email;
            EMP.PhoneNumber = employeeInfo.PhoneNumber;
            EMP.IsDeleted = false;
            EMP.CinemaID = cinema.CinemaID;
            EMP.Username = employeeInfo.Username;
            EMP.PasswordSalt = employeeInfo.PasswordSalt;
            EMP.PasswordHash = employeeInfo.PasswordHash;

            principal.Employees.Add(EMP);
            principal.SaveChanges();

            int RoleID = principal.Roles.Where(x => x.Name == "Director").FirstOrDefault().RoleID;

            EmployeeRoles employeeRoles = new EmployeeRoles
            {
                EmployeeID = EMP.EmployeeID,
                RoleID = RoleID,
                IsDeleted = false
            };
            principal.EmployeeRoles.Add(employeeRoles);
            principal.SaveChanges();

            return Ok( new { CinemaID=cinema.CinemaID, EmployeeID=EMP.EmployeeID, RoleID=employeeRoles.RoleID});
        }

        [HttpPut]
        [Route("PutCinemas")]
        public IHttpActionResult PutCinemas(CinemaInfo cinemaInfo)
        {
            var cinema = principal.Cinemas.Where(x => x.CinemaID == cinemaInfo.CinemaID).FirstOrDefault();
            cinema.Name = cinemaInfo.Name;
            cinema.Address = cinemaInfo.Address;

            principal.SaveChanges();
            return Ok(new { CinemaID= cinema.CinemaID });
        }
        [HttpPut]
        [Route("MakeCinemasInactive")]
        public IHttpActionResult MakeCinemaInactive(CinemaInfo cinemaInfo)
        {
            var cinema = principal.Cinemas.Where(x => x.CinemaID == cinemaInfo.CinemaID).FirstOrDefault();
            cinema.IsDeleted = true;
            
            principal.SaveChanges();
            return Ok(new { CinemaID = cinema.CinemaID });
        }

        [HttpGet]
        [Route("GetVirtualPointsPackets")]
        public IHttpActionResult GetVirtualPointsPackets()
        {
            var vps = principal.VirtualPointsPackets.Where(x => (bool)!x.IsDeleted).ToList();
            if (vps.Count == 0)
                return NotFound();  
            return Ok(vps.Select(x=>new {
                VirtualPointsPacketID = x.VirtualPointsPacketID,
                Amount=x.Amount
            }));
        }

        [HttpPost]
        [Route("PostVirtualPointsPackets")]
        public IHttpActionResult PostVirtualPointsPackets(dynamic obj)
        {
            decimal Amount = obj.Amount;
            var VPSameAmount = principal.VirtualPointsPackets
                .Where(x => (bool)!x.IsDeleted
                           && x.Amount == Amount).FirstOrDefault();
            if (VPSameAmount != null)
                return Content(HttpStatusCode.BadRequest, ExceptionsMapper[ExceptionType.VirtualPointsPacketsUniquenessAmountViolation]);

            var VPP = new VirtualPointsPackets
            {
                Amount = Amount,
                IsDeleted=false
            };
            principal.VirtualPointsPackets.Add(VPP);
            principal.SaveChanges();

            return Ok(new { VirtualPointsPacketID= VPP.VirtualPointsPacketID });
        }

        [HttpPut]
        [Route("PutVirtualPointsPackets")]
        public IHttpActionResult PutVirtualPointsPackets(dynamic obj)
        {
            int VPPid = obj.VirtualPointsPacketID;
            var VPP = principal.VirtualPointsPackets.Where(x =>x.VirtualPointsPacketID == VPPid).FirstOrDefault();
            VPP.IsDeleted = true;
            principal.SaveChanges();

            return Ok(new { VirtualPointsPacketID = VPP.VirtualPointsPacketID });
        }


    }

    public class CinemaInfo
    {
        public int CinemaID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int CinemaHallTotal { get; set; }
        public int EmployeeTotal { get; set; }
        public int CityID { get; set; }

    }
    public class EmployeeInfo
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CityBirthID { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Gender { get; set; }
        public string CurriculumVitae { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool IsDeleted { get; set; }
        public int CinemaID { get; set; }


    }
    public class CinemaDirectorComplex
    {
        public CinemaInfo cinemaInfo { get; set; }
        public EmployeeInfo employeeInfo { get; set; }

    }

}
