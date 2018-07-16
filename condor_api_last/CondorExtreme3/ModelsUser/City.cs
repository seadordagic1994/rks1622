using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondorExtreme3.ModelsUser
{
    [Table("Cities")]
    public class City
    {
        [Key]
        public int CityID { get; set; }
        public string Name { get; set; }
     
        public string PostalCode { get; set; }
   
        [ForeignKey("Country")]
        public int  CountryID { get; set; }

        public virtual Country Country { get; set; }

        public string Guid { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<RegisteredVisitor> RegisteredVisitors { get; set; }


    }
}