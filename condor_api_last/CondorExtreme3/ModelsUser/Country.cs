using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;


namespace CondorExtreme3.ModelsUser
{
    [Table("Countries")]
    public class Country
    {
        [Key]
        public int CountryID { get; set; }     
        public string Name { get; set; }
        public string Guid { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}