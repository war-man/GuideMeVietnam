//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VietTravel.DBModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class USER
    {
        public USER()
        {
            this.PLAN_TRIP = new HashSet<PLAN_TRIP>();
        }

        [System.ComponentModel.DataAnnotations.Key]
        public short userID { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public Nullable<short> accountID { get; set; }
    
        public virtual ACCOUNT ACCOUNT { get; set; }
        public virtual ICollection<PLAN_TRIP> PLAN_TRIP { get; set; }
    }
}
