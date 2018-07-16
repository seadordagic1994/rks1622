using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondorExtreme3.ModelsLocalDB
{
    [Table("SeatColumns")]
    public class SeatColumn
    {
        [Key]
        public int SeatColumnID { get; set; }

        public virtual ICollection<Seat> Seats { get; set; }

        public bool IsDeleted { get; set; }


    }
}