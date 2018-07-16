using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using CondorExtreme3.Tools;

namespace CondorExtreme3.ModelsUser
{
    [Table("RegisteredVisitors")]
    public class RegisteredVisitor 
    {
        [Key]
        public int RegisteredVisitorID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [ForeignKey("City")]
        public int CityID { get; set; }
        public virtual City City { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

       
        public int VirtualPointsTotal { get; set; }
        public virtual ICollection<SalesOrderVirtualPoints> SalesOrderVirtualPoints { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }


        public string Guid { get; set; }
        public bool IsDeleted { get; set; }


    }
}
