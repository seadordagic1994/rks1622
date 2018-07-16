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
    [Table("Visitors")]
    public class Visitor
    {
        [Key]
        public int VisitorID { get; set; }
      
        public string PhoneNumber { get; set; }
      
        public string ActivationCode { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }

        public bool IsDeleted { get; set; }

    }
}
