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
    
    public partial class MovieRoles
    {
        public int MovieID { get; set; }
        public int ActorID { get; set; }
        public string RoleSynopsis { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Actors Actors { get; set; }
        public virtual Movies Movies { get; set; }
    }
}
