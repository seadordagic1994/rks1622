using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;





namespace CondorExtreme3.ModelsLocalDB
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }
      
        public string FirstName { get; set; }
       
        public string LastName { get; set; }

        [ForeignKey("City")]
        public int CityID { get; set; }

        public virtual City City { get; set; }

       
        public string EmailAddress { get; set; }
   
        public string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }
        public bool Gender { get; set; }
        public DateTime HireDate { get; set; }
        public double Salary { get; set; }
        [Required(ErrorMessage = "Required or invalid")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required or invalid")]
        public string Password { get; set; }
       
        public string Picture { get; set; }


        public virtual ICollection<EmployeeRole> EmployeesRoles { get; set;  }


       

        public bool IsDeleted { get; set; }



    }
}
