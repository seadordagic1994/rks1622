using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;


namespace CondorExtreme3.ModelsLocalDB
{
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }
        public int SeatID { get; set; }
        public int ReservationID { get; set; }
        public virtual Seat Seat { get; set; }
        public virtual Reservation Reservation { get; set; }
        public virtual ICollection<Discount> Discounts { get; set; }
        public decimal TicketPrice { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalTicketPrice { get; set; }
        public bool IsSold { get; set; }
        public bool IsDeleted { get; set; }
    }
}
