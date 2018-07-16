using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondorExtreme3.ModelsLocalDB
{
    [Table("DefinedDateTimes")]
    public class DefinedDateTime
    {
        [Key]
        public DateTime DateTimeStart { get; set; }

        public virtual ICollection<Projection> Projections { get; set; }

        public bool IsDeleted { get; set; }


    }
}