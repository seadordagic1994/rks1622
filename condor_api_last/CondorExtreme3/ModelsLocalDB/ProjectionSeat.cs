using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondorExtreme3.ModelsLocalDB
{
    [Table("ProjectionSeats")]
    public class ProjectionSeat
    {
        [Column(Order =0),Key][ForeignKey("Projection")]
        public int ProjectionID { get; set; }
        [Column(Order = 1), Key][ForeignKey("Seat")]
        public int SeatID { get; set; }

        public virtual Projection Projection { get; set; }

        public virtual Seat Seat  { get; set; }
        
        public bool IsReserved { get; set; }

        public bool IsDeleted { get; set; }


    }
}