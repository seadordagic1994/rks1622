using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CondorExtreme3.ModelsLocalDB
{
    [Table("Addresses")]
    public class Address
    {
        [Key]
        public int AddressID { get; set; }
         
        public string AddressLine1 { get; set; }
      
        [ForeignKey("City")]
        public int CityID { get; set; }

        public virtual City City { get; set; }

        public bool IsDeleted { get; set; }

    }
}