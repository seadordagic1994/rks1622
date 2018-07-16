using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondorExtreme3.ModelsUser
{
    [Table("SalesOrderVirtualPoints")]
    public class SalesOrderVirtualPoints
    {
        [Key]
        public int SalesOrderNumber { get; set; }

        [ForeignKey("VirtualPointsPack")]   
        public int VirtualPointsPackID { get; set; }
        public virtual VirtualPointsPack VirtualPointsPack { get; set; }

        [ForeignKey("RegisteredVisitor")]     
        public int RegisteredVisitorID { get; set; }
        public virtual RegisteredVisitor RegisteredVisitor { get; set; }

        public string Guid { get; set; }
        public bool IsDeleted { get; set; }

    }
}
