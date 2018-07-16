using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondorExtreme3.ModelsLocalDB
{
    [Table("Directors")]
    public class Director
    {
        [Key]
        public int DirectorID { get; set; }
       
        public string FirstName { get; set; }
      
        public string LastName { get; set; }

        public virtual ICollection<MovieDirection> MovieDirections { get; set; }

        public bool IsDeleted { get; set; }



    }
}