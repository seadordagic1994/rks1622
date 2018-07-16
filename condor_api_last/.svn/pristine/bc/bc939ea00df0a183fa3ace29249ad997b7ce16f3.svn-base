using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondorExtreme3.ModelsLocalDB
{
    [Table("Genres")]
    public class Genre
    {
        [Key]
        public int GenreID { get; set; }
        
        public string GenreName { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

        public bool IsDeleted { get; set; }


    }
}