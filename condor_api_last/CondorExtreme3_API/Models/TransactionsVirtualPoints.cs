//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CondorExtreme3_API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TransactionsVirtualPoints
    {
        public int TransactionVirtualPointsID { get; set; }
        public int VirtualPointsPacketID { get; set; }
        public int RVisitorID { get; set; }
        public System.DateTime TransactionDate { get; set; }
    
        public virtual RVisitors RVisitors { get; set; }
        public virtual VirtualPointsPackets VirtualPointsPackets { get; set; }
    }
}