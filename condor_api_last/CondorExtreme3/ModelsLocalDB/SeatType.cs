using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondorExtreme3.ModelsLocalDB
{
    [Table("SeatTypes")]
    public class SeatType
    {
        [Key]
        public int SeatTypeID { get; set; }
      
        public string SeatTypeName { get; set; }

        public bool IsDeleted { get; set; }

    }
}