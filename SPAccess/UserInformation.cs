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
    
    public partial class UserInformation
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public System.DateTime UpdateTime { get; set; }
    
        public virtual UserIdentity UserIdentity { get; set; }
    }
}
