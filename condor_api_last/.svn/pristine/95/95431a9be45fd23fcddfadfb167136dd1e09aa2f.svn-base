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
    [Table("PaymentMethods")]
    public class PaymentMethod
    {
        [Key]
        public int PaymentMethodID { get; set; }
        
        public string MethodName { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }

        public bool IsDeleted { get; set; }

    }
}
