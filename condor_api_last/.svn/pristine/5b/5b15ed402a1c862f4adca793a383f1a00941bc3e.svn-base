using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CondorExtreme3.ModelsLocalDB
{
    [Table("DiscountTypes")]
    public class DiscountType
    {
        [Key]
        public int DiscountTypeID { get; set; }
      
        public string DiscountName { get; set; }

        public decimal DiscountAmount { get; set; }

        public virtual ICollection<Discount> Discounts { get; set; }

        public bool IsDeleted { get; set; }


    }
}
