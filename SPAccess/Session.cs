//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SPAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Session
    {
        public int SessionID { get; set; }
        public int UserID { get; set; }
        public string Token { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public Nullable<System.DateTime> ExpiredTime { get; set; }
    
        public virtual UserIdentity UserIdentity { get; set; }
    }
}
