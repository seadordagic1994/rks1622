using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondorExtreme3.ModelsLocalDB
{
    [Table("Seats")]
    public class Seat
    {
        [Key]
        public int SeatID { get; set; }

       [ForeignKey("CinemaHall")]
        public int CinemaHallID { get; set; }
       [ForeignKey("SeatRow")]
        public string SeatRowID { get; set; }
       [ForeignKey("SeatColumn")]
        public int SeatColumnID { get; set; }


        public virtual CinemaHall CinemaHall { get; set; }

        public virtual SeatRow SeatRow { get; set; }

        public virtual SeatColumn SeatColumn { get; set; }

        [ForeignKey("SeatType")]
        public int SeatTypeID { get; set; }

        public virtual SeatType SeatType { get; set; }

        public virtual ICollection<ProjectionSeat> ProjectionSeats { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

        public bool IsDeleted { get; set; }


    }
}