using CondorExtreme3_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Web.Http.Results;
using System.Web.Http.Description;
using static CondorExtreme3_API.Helper.FrameworkExceptionHandler;

namespace CondorExtreme3_API.Controllers
{
    [RoutePrefix("api/Employees")]
    public class EmployeesController : ApiController
    {
        public CondorDBXEntities principal= new CondorDBXEntities();

        public IHttpActionResult GetEmployees()
        {
            List<Employees> Emps = principal.Employees.Where(x => !x.IsDeleted).ToList();
            if (Emps.Count == 0)
                return NotFound();

            return Ok(Emps.Select(x => new
            {
                EmployeeID = x.EmployeeID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                City = x.Cities.Name,
                BirthDate = x.BirthDate,
                GenderStr = (!x.Gender) ? "Male" : "Female",
                Gender=x.Gender,
                CurriculumVitae=x.CurriculumVitae,
                Email=x.Email,
                PhoneNumber=x.PhoneNumber,
                Username=x.Username,
                PasswordHash=x.PasswordHash,
                PasswordSalt = x.PasswordSalt

            }).ToList());
        }
        [Route("GetEmployeesByCinema/{CinemaID}")]
        public IHttpActionResult GetEmployeesByCinema(int CinemaID)
        {
            List<Employees> Emps = principal.Employees.Where(x => !x.IsDeleted && x.CinemaID == CinemaID).ToList();
            if (Emps.Count == 0)
                return NotFound();

            return Ok(Emps.Select(x => new
            {
                EmployeeID = x.EmployeeID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                CityBirthID = x.CityBirthID,
                City = x.Cities.Name,
                BirthDate = x.BirthDate,
                GenderStr = (!x.Gender) ? "Male" : "Female",
                Gender= x.Gender,
                CurriculumVitae = x.CurriculumVitae,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Username = x.Username,
                PasswordHash = x.PasswordHash,
                PasswordSalt = x.PasswordSalt

            }).ToList());
        }

        [HttpPost]
        [Route("SearchEmployees")]
        public IHttpActionResult SearchEmployees(EmployeeInfoParameters employeeInfoParameters)
        {
            int CinemaID = employeeInfoParameters.CinemaID;
            string FirstName= employeeInfoParameters.FirstName;
            string LastName = employeeInfoParameters.LastName;
            int CityBirthID = employeeInfoParameters.CityBirthID;
            int GenderID = employeeInfoParameters.GenderID;

            var Genders = (GenderID == 0) ? new List<bool> { false, true } :
                         (GenderID == 1) ? new List<bool> { false } :
                        new List<bool> { true };

            List<int> listCities = new List<int>();


            listCities.AddRange((CityBirthID == 0) ? principal.Cities.Select(x => x.CityID).ToList() : principal.Cities.Where(x => x.CityID == CityBirthID).Select(x => x.CityID).ToList());


            List<Employees> listEmployees = principal.Employees
                .Where(x => !x.IsDeleted
                    && x.CinemaID == employeeInfoParameters.CinemaID
                    && x.FirstName.Contains(FirstName)
                    && x.LastName.Contains(LastName)
                    && Genders.Contains(x.Gender)
                    && listCities.Contains(x.Cities.CityID)
                 ).ToList();      
            if (listEmployees.Count == 0)
                  return NotFound();
            return Ok(listEmployees.Select(x => new
            {
                EmployeeID = x.EmployeeID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                CityBirthID = x.CityBirthID,              
                CityName = x.Cities.Name,
                BirthDate = x.BirthDate, 
                Gender = x.Gender,
                CurriculumVitae = x.CurriculumVitae,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Username = x.Username,
                IsDeleted=x.IsDeleted
            }).ToList());  
        }


        [Route("{EmployeeID}")]
        public IHttpActionResult GetEmployees(int EmployeeID)
        {
            Employees E = principal.Employees.Where(x => x.EmployeeID == EmployeeID && !x.IsDeleted).FirstOrDefault();

            if (E == null)
                return NotFound();
           
            return Ok( new {
                EmployeeID = E.EmployeeID,
                FirstName = E.FirstName,
                LastName = E.LastName,
                City = E.Cities.Name,
                BirthDate = E.BirthDate,
                Gender = (!E.Gender) ? "Male" : "Female",
                CurriculumVitae = E.CurriculumVitae,
                Email = E.Email,
                PhoneNumber = E.PhoneNumber,
                Username = E.Username,
                PasswordSalt = E.PasswordSalt,
                PasswordHash = E.PasswordHash
            });
        }

        [Route("GetEmployees/{Username}")]
        public IHttpActionResult GetEmployees(string Username)
        {
            Employees E = principal.Employees.Where(x => x.Username == Username && !x.IsDeleted).FirstOrDefault();
            if(E == null)
                return NotFound();

            return Ok( new {
                EmployeeID = E.EmployeeID,
                FirstName = E.FirstName,
                LastName = E.LastName,
                City = E.Cities.Name,
                BirthDate = E.BirthDate,
                Gender = (!E.Gender) ? "Male" : "Female",
                CurriculumVitae = E.CurriculumVitae,
                Email = E.Email,
                PhoneNumber = E.PhoneNumber,
                Username = E.Username,
                PasswordSalt=E.PasswordSalt,
                PasswordHash=E.PasswordHash
            });
        }


        [HttpPost]
        [Route("PostEmployees")]
        public IHttpActionResult PostEmployees(Employees obj)
        {

            var employeeSamePhoneNumber = principal.Employees.Where(x => x.PhoneNumber == obj.PhoneNumber && !x.IsDeleted).FirstOrDefault();
            if (employeeSamePhoneNumber != null)
                return Content(HttpStatusCode.BadRequest, ExceptionsMapper[ExceptionType.PhoneUniquenessViolation]);

            var employeeSameEmail = principal.Employees.Where(x => x.Email == obj.Email && !x.IsDeleted).FirstOrDefault();
            if (employeeSameEmail != null)
                return Content(HttpStatusCode.BadRequest, ExceptionsMapper[ExceptionType.EmailUniquenessViolation]);

            var employeeSameUsername = principal.Employees.Where(x => x.Username == obj.Username && !x.IsDeleted).FirstOrDefault();
            if (employeeSameUsername != null)
                return Content(HttpStatusCode.BadRequest, ExceptionsMapper[ExceptionType.UsernameUniquenessViolation]);

            Employees EMP = new Employees();

            EMP.FirstName = obj.FirstName;
            EMP.LastName = obj.LastName;
            EMP.CityBirthID = obj.CityBirthID;
            EMP.BirthDate = obj.BirthDate;
            EMP.Gender = obj.Gender;
            EMP.CurriculumVitae = obj.CurriculumVitae;
            EMP.Email = obj.Email;
            EMP.PhoneNumber = obj.PhoneNumber;     
            EMP.IsDeleted= obj.IsDeleted;
            EMP.CinemaID = obj.CinemaID;
            EMP.Username = obj.Username;
            EMP.PasswordSalt = obj.PasswordSalt;
            EMP.PasswordHash = obj.PasswordHash;

            principal.Employees.Add(EMP);
            principal.SaveChanges();

            return Ok( new { EmployeeID=EMP.EmployeeID });
        }
        [HttpPut]
        [Route("PutEmployees")]
        public IHttpActionResult PutEmployees(Employees obj)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var employeeSamePhoneNumber = principal.Employees.Where(x => x.PhoneNumber == obj.PhoneNumber && !x.IsDeleted && x.EmployeeID!=obj.EmployeeID).FirstOrDefault();
            if (employeeSamePhoneNumber != null)
                return Content(HttpStatusCode.BadRequest, ExceptionsMapper[ExceptionType.PhoneUniquenessViolation]);
            var employeeSameEmail = principal.Employees.Where(x => x.Email == obj.Email && !x.IsDeleted && x.EmployeeID != obj.EmployeeID).FirstOrDefault();
            if (employeeSameEmail != null)
                return Content(HttpStatusCode.BadRequest, ExceptionsMapper[ExceptionType.EmailUniquenessViolation]);

            Employees EMP = principal.Employees.Where(x => x.EmployeeID == obj.EmployeeID).FirstOrDefault();

            EMP.FirstName = obj.FirstName;
            EMP.LastName = obj.LastName;
            EMP.CityBirthID = obj.CityBirthID;
            EMP.BirthDate = obj.BirthDate;
            EMP.Gender = obj.Gender;
            EMP.CurriculumVitae = obj.CurriculumVitae;
            EMP.Email = obj.Email;
            EMP.PhoneNumber = obj.PhoneNumber;
                
            principal.SaveChanges();
            return Ok(new { EmployeeID=obj.EmployeeID });
        }


        [Route("GetEmployeeRoles/{EmployeeID}")]
        public IHttpActionResult GetEmployeeRoles(int EmployeeID)
        {
            Employees EMP = principal.Employees.Where(x => x.EmployeeID == EmployeeID && !x.IsDeleted).FirstOrDefault();
            if (EMP == null)
                return NotFound();
            List<EmployeeRoles> ERoles = principal.EmployeeRoles.Where(x => x.EmployeeID == EMP.EmployeeID && !x.IsDeleted).ToList();
            return Ok(ERoles.Select(x => new {
                EmployeeID=x.EmployeeID,
                RoleID=x.RoleID,
                RoleName=x.Roles.Name,
                Priority=x.Roles.Priority
            }).ToList());
        }
        [HttpGet]
        [Route("GetCredentials/{EmployeeID}")]
        public IHttpActionResult GetCredentials(int EmployeeID)
        {
            var Credentials = principal.Employees.Where(x => x.EmployeeID == EmployeeID).Select(x => new
            {
                Username=x.Username,
                PasswordHash=x.PasswordHash,
                PasswordSalt=x.PasswordSalt
            }).FirstOrDefault();

            if (Credentials == null)
                return NotFound();
            return Ok(Credentials);
        }




        [HttpGet]
        [Route("GetCitiesFilter")]
        public IHttpActionResult GetCitiesFilter()
        {
            var listCities = principal.Cities.ToList().Select(x => new
            {
                CityID = x.CityID,
                Name = x.Name
            }).ToList();

            if (listCities.Count == 0)
                return NotFound();

            var finalList = new List<dynamic>();
            finalList.Add(new { CityID = 0, Name = "(All Cities)" });
            finalList.AddRange(listCities);

            return Ok(finalList);

        }
        [HttpPut]
        [Route("MakeEmployeesInactive")]
        public IHttpActionResult MakeEmployeesInactive(dynamic obj)
        {
            int EmployeeID = (int)obj.EmployeeID;
            var Employee = principal.Employees.Where(x => x.EmployeeID == EmployeeID).FirstOrDefault();
            Employee.IsDeleted = true;
            principal.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("GetEmploymentTypes")]
        public IHttpActionResult GetEmploymentTypes()
        {            
            var listEmploymentTypes = principal.EmploymentTypes.ToList().Select(x => new
            {
                EmploymentTypeID = x.EmploymentTypeID,
                Name = x.Name
            }).ToList();

            if (listEmploymentTypes.Count == 0)
                return NotFound();

            var finalList = new List<dynamic>();
            finalList.Add(new { EmploymentTypeID = 0, Name = "(All Employment Types)" });
            finalList.AddRange(listEmploymentTypes);
            return Ok(finalList);
        }

        [HttpPost]
        [Route("SearchEmployments")]
        public IHttpActionResult SearchEmployments(EmploymentInfoParameters employmentInfoParameters)
        {
            int CinemaID = employmentInfoParameters.CinemaID;
            string FirstName = employmentInfoParameters.FirstName;
            string LastName = employmentInfoParameters.LastName;
            int EmploymentTypeID = employmentInfoParameters.EmploymentTypeID;
        
            List<int> listEmploymentTypes = new List<int>();
            listEmploymentTypes.AddRange((EmploymentTypeID == 0) ? principal.EmploymentTypes.Select(x => x.EmploymentTypeID).ToList() : principal.EmploymentTypes.Where(x => x.EmploymentTypeID == EmploymentTypeID).Select(x => x.EmploymentTypeID).ToList());

            List<Employments> listEmployments = principal.Employments
                .Where(x => !x.IsDeleted
                    && x.Employees.Cinemas.CinemaID == employmentInfoParameters.CinemaID
                    && x.Employees.FirstName.Contains(FirstName)
                    && x.Employees.LastName.Contains(LastName)   
                    && listEmploymentTypes.Contains(x.EmploymentTypeID)   
                 ).ToList();
            if (listEmployments.Count == 0)
                return NotFound();

            var rndlist = listEmployments.Select(x => new
            {
                EmploymentID = x.EmploymentID,
                EmployeeID=x.EmployeeID,
                FirstName = x.Employees.FirstName,
                LastName = x.Employees.LastName,
                EmploymentTypeID = x.EmploymentTypeID,
                EmploymentTypeName = x.EmploymentTypes.Name,
                HireDate = x.HireDate,
                CurrentSalary = x.CurrentSalary,
                BankAccountNumber = x.BankAccountNumber,
                BankID = x.BankID,
                BankName = x.Banks.Name,
                IsDeleted = x.IsDeleted
            }).ToList();

            return Ok(rndlist);
        }

        [HttpGet]
        [Route("GetEmployeesForEmployment/{CinemaID}")]
        public IHttpActionResult GetEmployeesForEmployment(int CinemaID)
        {
            List<Employees> listEmployees = principal.Employees.Where(x => x.CinemaID == CinemaID && !x.IsDeleted).ToList();
            if (listEmployees.Count == 0)
                return NotFound();                
            return Ok(listEmployees.Select(x=> new {
                EmployeeID=x.EmployeeID,
                FullName=x.FirstName+" "+x.LastName

            }).ToList());
        }

        [HttpGet]
        [Route("GetEmploymentTypesForEmployment")]
        public IHttpActionResult GetEmploymentTypesForEmployment()
        {
            List<EmploymentTypes> listEmploymentTypes = principal.EmploymentTypes.ToList();
            if (listEmploymentTypes.Count == 0)
                return NotFound();

            return Ok(listEmploymentTypes.Select(x => new {
                EmploymentTypeID = x.EmploymentTypeID,
                Name= x.Name
            }).ToList());
        }

        [HttpGet]
        [Route("GetBanks")]
        public IHttpActionResult GetBanks()
        {
            List<Banks> listBanks = principal.Banks.ToList();
            if (listBanks.Count == 0)
                return NotFound();

            return Ok(listBanks.Select(x => new {
                BankID = x.BankID,
                Name = x.Name
            }).ToList());
        }

        [HttpPost]
        [Route("PostEmployments")]
        public IHttpActionResult PostEmployments(Employments obj)
        {
            var employmentSameBankAccount = principal.Employments
                .Where(x => x.BankAccountNumber == obj.BankAccountNumber
                           && x.EmployeeID!=obj.EmployeeID
                           && !x.IsDeleted).FirstOrDefault();
            if (employmentSameBankAccount != null)
                return Content(HttpStatusCode.BadRequest, ExceptionsMapper[ExceptionType.BankAccountNumberUniquenessViolation]);

            var SameEmployment = principal.Employments
                .Where(x => !x.IsDeleted && x.EmployeeID == obj.EmployeeID && x.EmploymentTypeID == obj.EmploymentTypeID)
                .FirstOrDefault();
            if (SameEmployment != null)
                return Content(HttpStatusCode.BadRequest, ExceptionsMapper[ExceptionType.EmployeeEmploymentsDuplicate]);


            Employments empl = new Employments();
            empl.EmployeeID = obj.EmployeeID;
            empl.HireDate = obj.HireDate;
            empl.CurrentSalary = obj.CurrentSalary;
            empl.EmploymentTypeID = obj.EmploymentTypeID;
            empl.BankAccountNumber = obj.BankAccountNumber;
            empl.BankID = obj.BankID;
            empl.IsDeleted = false;
            principal.Employments.Add(empl);
            principal.SaveChanges();

            return Ok( new { EmploymentID=empl.EmploymentID, EmployeeID=empl.EmployeeID, EmploymentTypeID = empl.EmploymentTypeID});
        }

        [HttpPut]
        [Route("PutEmployments")]
        public IHttpActionResult PutEmployments(Employments obj)
        {
            
            Employments empl = principal.Employments.Where(x => x.EmploymentID == obj.EmploymentID).FirstOrDefault();

            if (empl.EmploymentTypeID != obj.EmploymentTypeID)
            {
                var EmploymentEmployeeDuplicate = principal.Employments
                    .Where(x => !x.IsDeleted
                              && obj.EmploymentTypeID == x.EmploymentTypeID
                              && obj.EmployeeID == x.EmployeeID)
                              .FirstOrDefault();
                if (EmploymentEmployeeDuplicate != null)
                    return Content(HttpStatusCode.BadRequest, ExceptionsMapper[ExceptionType.EmployeeEmploymentsDuplicate]);
            }
            if(empl.BankAccountNumber != obj.BankAccountNumber)
            {
                var EmployeeWithSameBankAccountNumber = principal.Employments
                    .Where(x => !x.IsDeleted
                             && x.EmployeeID != obj.EmployeeID
                             && x.BankAccountNumber == obj.BankAccountNumber)
                          .FirstOrDefault();
                if (EmployeeWithSameBankAccountNumber != null)
                    return Content(HttpStatusCode.BadRequest, ExceptionsMapper[ExceptionType.BankAccountNumberUniquenessViolation]);
            }
        
                                 
            empl.HireDate = obj.HireDate;
            empl.CurrentSalary = obj.CurrentSalary;
            empl.EmploymentTypeID = obj.EmploymentTypeID;
            empl.BankAccountNumber = obj.BankAccountNumber;
            empl.BankID = obj.BankID;          
            principal.SaveChanges();
            return Ok(new { EmploymentID = empl.EmploymentID, EmployeeID = empl.EmployeeID, EmploymentTypeID = empl.EmploymentTypeID });
        }
        [HttpPut]
        [Route("MakeEmploymentsInactive")]
        public IHttpActionResult MakeEmploymentsInactive(dynamic obj)
        {
            int EmploymentID = (int)obj.EmploymentID;
            var Employment = principal.Employments.Where(x => x.EmploymentID == EmploymentID).FirstOrDefault();
            Employment.IsDeleted = true;
            principal.SaveChanges();
            return Ok();
        }
        [HttpGet]
        [Route("GetEmployeePayments/{EmploymentID}")]
        public IHttpActionResult GetEmployeePayments(int EmploymentID)
        {
            var listEmployeePayments = principal.EmployeePayments.Where(x => x.EmploymentID == EmploymentID).ToList();
            if (listEmployeePayments.Count == 0)
                return NotFound();
            return Ok(listEmployeePayments.Select(x=>new {
                EmployeePaymentID=x.EmployeePaymentID,
                EmploymentID= x.EmploymentID,
                TransactionDate= x.TransactionDate,
                Amount=x.Amount
            }).ToList());
        }
        [HttpPost]
        [Route("PostEmployeePayments")]
        public IHttpActionResult PostEmployeePayments(EmployeePayments obj)
        {
            var emplpay = new EmployeePayments();
            emplpay.EmploymentID = obj.EmploymentID;
            emplpay.TransactionDate = obj.TransactionDate;
            emplpay.Amount = obj.Amount;
            principal.EmployeePayments.Add(emplpay);
            principal.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("GetRoles")]
        public IHttpActionResult GetRoles()
        {
            List<Roles> emproles = principal.Roles.Where(x=>x.Name!="Director").ToList();
            if (emproles.Count == 0)
                return NotFound();
            return Ok(emproles.Select(x => new
            {
                RoleID=x.RoleID,
                Name = x.Name
            }).ToList());
        }
        [HttpPost]
        [Route("PostEmployeeRoles")]
        public IHttpActionResult PostEmployeeRoles(dynamic obj)
        {
            int EmployeeID = obj.EmployeeID;
            int RoleID = obj.RoleID;

            var empCheck = principal.EmployeeRoles.Where(x => x.EmployeeID == EmployeeID && x.RoleID == RoleID).FirstOrDefault();
            if (empCheck != null)
                return BadRequest();

            var EmployeeRole = new EmployeeRoles
            {
                EmployeeID = EmployeeID,
                RoleID = RoleID,
                IsDeleted = false
            };
            principal.EmployeeRoles.Add(EmployeeRole);
            principal.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("RemoveEmployeeRoles")]
        public IHttpActionResult RemoveEmployeeRoles(dynamic obj)
        {
            int EmployeeID = obj.EmployeeID;
            int RoleID = obj.RoleID;
            var empCheck = principal.EmployeeRoles.Where(x => x.EmployeeID == EmployeeID && x.RoleID == RoleID).FirstOrDefault();
            if (empCheck == null)
                return NotFound();
            principal.EmployeeRoles.Remove(empCheck);
            principal.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("ChangeAccountInfo")]
        public IHttpActionResult ChangeAccountInfo(Credentials credentials)
        {
            var Employee = principal.Employees.Where(x => !x.IsDeleted && x.EmployeeID == credentials.EmployeeID).FirstOrDefault();

            if (Employee.Username != credentials.Username)
            {
                var EmployeeSameUsername = principal.Employees
                    .Where(x => !x.IsDeleted &&
                            x.Username == credentials.Username &&
                            x.EmployeeID != credentials.EmployeeID).FirstOrDefault();
                if (EmployeeSameUsername != null)
                    return Content(HttpStatusCode.BadRequest, ExceptionsMapper[ExceptionType.UsernameUniquenessViolation]);
                
            }
            Employee.Username = credentials.Username;

            if (credentials.changePassword)
            {
                Employee.PasswordHash = credentials.PasswordHash;
                Employee.PasswordSalt = credentials.PasswordSalt;
            }       
            principal.SaveChanges();
            return Ok();
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (principal != null)   
                    principal.Dispose();
                
            }
            base.Dispose(disposing);
        }


       

    }
    public class EmployeeInfoParameters
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public int CinemaID { get; set; }
        public int CityBirthID { get; set; }
        public int GenderID { get; set; }

    }
    public class EmploymentInfoParameters
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CinemaID { get; set; }
        public int EmploymentTypeID { get; set; }

    }

    public class Credentials
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public int EmployeeID { get; set; }

        public bool changePassword { get; set; }

    }

}
