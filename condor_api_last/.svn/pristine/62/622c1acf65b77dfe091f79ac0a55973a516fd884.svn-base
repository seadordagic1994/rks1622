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
    [Table("Roles")]
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        
        public string RoleName { get; set; }

        public virtual ICollection<EmployeeRole> EmployeesRoles { get; set; }

        public bool IsDeleted { get; set; }

    }
}
