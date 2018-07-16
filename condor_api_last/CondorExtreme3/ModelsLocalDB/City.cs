using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CondorExtreme3.ModelsLocalDB
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
       
        public virtual ICollection<Address> Addresses { get; set; }
   
        public bool IsDeleted { get; set; }

    }
}