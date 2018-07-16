using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondorExtreme3.ModelsLocalDB
{
    [Table("TechnologyTypes")]
    public class TechnologyType
    {
        [Key]
        public int TechnologyTypeID { get; set; }
        
        public string Name { get; set; }

      

        public virtual ICollection<CinemaHallTechnologyType> CinemaHallTechnologyTypes { get; set; }

        public virtual ICollection<Projection> Projections { get; set; }

        public bool IsDeleted { get; set; }

    }
}