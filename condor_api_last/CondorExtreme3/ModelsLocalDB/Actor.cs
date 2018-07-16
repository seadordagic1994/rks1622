using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondorExtreme3.ModelsLocalDB
{
    [Table("Actors")]
    public class Actor
    {
        [Key]
        public int ActorID { get; set; }
       
        public string FirstName { get; set; }
       
        public string LastName { get; set; }

        public virtual ICollection<MovieRole> MovieRoles { get; set; }

        public bool IsDeleted { get; set; }

    }
}