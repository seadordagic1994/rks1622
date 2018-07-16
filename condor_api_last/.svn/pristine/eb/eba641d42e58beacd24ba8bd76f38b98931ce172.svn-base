using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CondorExtreme3.ModelsLocalDB
{
    [Table("SeatRows")]
    public class SeatRow
    {
        [Key]
        public string SeatRowID { get; set; }


        public virtual ICollection<Seat> Seats { get; set; }

        public bool IsDeleted { get; set; }


    }
}