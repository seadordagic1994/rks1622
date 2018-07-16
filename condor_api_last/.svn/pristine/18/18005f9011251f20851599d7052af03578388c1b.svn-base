using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/*-----------------------------------+
 * Author: Ilhan Karic               |
 * Module: Users                     |
 * Last Modified: 1/10/2018 @ 19:36  |
 * Copyright: Condor Coorporation    |
 *-----------------------------------*/
namespace CondorExtreme3.Areas.Local.Models
{
    public class VisitorVM
    {
        [Key]
        public int VisitorID { get; set; }
        [Required(ErrorMessage="Enter the phone number"),MaxLength(50, ErrorMessage = "Phone number too long!")]
        public string PhoneNumber { get; set; }
        public string ActivationCode { get; set; }
        public string ActivationConfirmationCode { get; set; }
        public string ErrorMessage { get; set; } = "";
        public string ProjectionId { get; set; }
        public List<string> Seats { get; set; }
    }
}