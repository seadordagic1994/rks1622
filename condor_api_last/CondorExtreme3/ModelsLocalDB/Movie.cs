using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondorExtreme3.ModelsLocalDB
{
    [Table("Movies")]
    public class Movie
    {
        [Key]
        public int MovieID { get; set; }
      
        public string MovieName { get; set; }
      
        public string OriginalName { get; set; }
        
     
        [ForeignKey("Genre")]
        public int GenreID { get; set; }

        public virtual Genre Genre { get; set; }
     
        public int DurationInMinutes { get; set; }
     
        public string AgeRestriction { get; set; }
      
        public string ReleaseYear { get; set; }
        
        public string Synopsis { get; set; }
        
        public string Picture { get; set; }
        public string Trailler { get; set; }

        public bool IsCurrent { get; set; }


        public virtual ICollection<MovieRole> MovieRoles { get; set; }

        public virtual ICollection<MovieDirection> MovieDirections { get; set; }


        public virtual ICollection<Projection> Projections { get; set; }

        public bool IsDeleted { get; set; }


    }
}