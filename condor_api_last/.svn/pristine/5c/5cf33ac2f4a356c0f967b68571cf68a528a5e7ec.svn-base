using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CondorExtreme3.ModelsLocalDB
{
    [Table("Projections")]
    public class Projection
    {
        [Key]
        public int ProjectionID { get; set; }


      
        [ForeignKey("Movie")]
        public int MovieID { get; set; }
       [ForeignKey("CinemaHall")]
        public int CinemaHallID { get; set; }
       [ForeignKey("DefinedDateTime")]
        public DateTime DateTimeStart { get; set; }

        public virtual Movie Movie { get; set; }

        public virtual CinemaHall CinemaHall { get; set; }

        public virtual DefinedDateTime DefinedDateTime { get; set; }
      
        public DateTime DateTimeEnd { get; set; }

        [ForeignKey("TechnologyType")]
        public int TechnologyTypeID { get; set; }

        public virtual TechnologyType TechnologyType { get; set; }
        
        public decimal TicketPrice { get; set; }
        

        public virtual ICollection<ProjectionSeat> ProjectionSeats { get; set; }


        public virtual ICollection<Reservation> Reservations { get; set; }

        public bool IsDeleted { get; set; }


    }
}