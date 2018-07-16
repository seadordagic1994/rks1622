using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondorExtreme3.ModelsLocalDB
{
    [Table("MovieDirection")]
    public class MovieDirection
    {
        [Column(Order =0),Key,ForeignKey("Movie")]
        public int MovieID { get; set; }
        [Column(Order = 1), Key, ForeignKey("Director")]
        public int DirectorID { get; set; }

        public virtual Movie Movie { get; set; }

        public virtual Director Director { get; set; }

        public bool IsDeleted { get; set; }


    }
}