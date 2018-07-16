using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondorExtreme3.ModelsLocalDB
{
    [Table("EmployeesRoles")]
    public class EmployeeRole
    {
        
        [Column(Order = 1), Key,ForeignKey("Role")]
        public int RoleID { get; set; }
        [Column(Order = 2), Key, ForeignKey("Employee")]
        public int EmployeeID { get; set;}

       public virtual Role Role { get; set; }
       public virtual Employee Employee { get; set; }

       public bool IsDeleted { get; set; }

    }
}
