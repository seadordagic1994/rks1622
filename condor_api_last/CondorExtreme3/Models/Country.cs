using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;


namespace CondorExtreme3.Models
{
    [Table("Countries")]
    public class Country
    {
        [Key]
        public int CountryID { get; set; }
      
        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; set; }

        public bool IsDeleted { get; set; }


    }
}