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
    [Table("Discounts")]
    public class Discount
    {
        [Column(Order =0),Key,ForeignKey("DiscountType")]
        public int DiscountTypeID { get; set; }
        [Column(Order = 1), Key, ForeignKey("Ticket")]
        public int TicketID { get; set; }

       public virtual DiscountType DiscountType { get; set; }

       public virtual Ticket Ticket { get; set; }

       public bool IsDeleted { get; set; }



    }
}