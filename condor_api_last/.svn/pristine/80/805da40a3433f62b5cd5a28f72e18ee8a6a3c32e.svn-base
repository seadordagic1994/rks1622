using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondorExtreme3.ModelsLocalDB
{
    [Table("CinemaHallsTechnologyTypes")]
    public class CinemaHallTechnologyType
    {

        [Column(Order =0),Key,ForeignKey("TechnologyType")]
        public int TechnologyTypeID { get; set; }
        [Column(Order = 1), Key, ForeignKey("CinemaHall")]
        public int CinemaHallID { get; set; }

        public virtual TechnologyType TechnologyType { get; set; }

        

        public virtual CinemaHall CinemaHall { get; set; }

        public bool IsDeleted { get; set; }

    }
}