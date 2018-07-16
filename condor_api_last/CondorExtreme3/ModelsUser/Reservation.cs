using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CondorExtreme3.ModelsUser
{
    [Table("Reservations")]
    public class Reservation
    {
        [Key]
        public int ReservationID { get; set; }
        public string ConnString { get; set; }
        [ForeignKey("RegisteredVisitor")]
        public int RegisteredVisitorID { get; set; }
        public virtual RegisteredVisitor RegisteredVisitor { get; set; }
        public string Guid { get; internal set; }
    }
}