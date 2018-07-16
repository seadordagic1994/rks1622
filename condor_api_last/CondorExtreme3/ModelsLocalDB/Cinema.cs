using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CondorExtreme3.DAL;
namespace CondorExtreme3.ModelsLocalDB
{
    [Table("Cinemas")]
    public class Cinema
    {
        [Key]
        public int CinemaID { get; set; }

        public string Name { get; set; }
        [ForeignKey("Address")]
        public int AddressID { get; set; }
        public string ConnectionString { get; set; }
        public virtual Address Address { get; set; }

        public bool IsDeleted { get; set; }


    }


}
