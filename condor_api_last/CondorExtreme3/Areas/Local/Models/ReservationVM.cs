using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/*-----------------------------------+
 * Author: Ilhan Karic               |
 * Module: Users                     |
 * Last Modified: 1/10/2018 @ 19:36  |
 * Copyright: Condor Coorporation    |
 *-----------------------------------*/
namespace CondorExtreme3.Areas.Local.Models
{
    public class ReservationVM
    {
        public int ReservationID { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string ConnectionString { get; set; }
    }
}