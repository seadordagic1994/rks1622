using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondorExtreme3.ModelsLocalDB
{
    [Table("CinemaHalls")]
    public class CinemaHall
    {
        [Key]
        public int CinemaHallID { get; set; }

        public string Name { get; set; }

        [ForeignKey("Cinema")]
        public int CinemaID { get; set; }       

        public virtual Cinema Cinema { get; set; }

        public virtual ICollection<Projection> Projections { get; set; }

        public virtual ICollection<Seat> Seats { get; set; }

        public bool IsDeleted { get; set; }

    }
}