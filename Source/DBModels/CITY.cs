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
    
    public partial class CITY
    {
        [System.ComponentModel.DataAnnotations.Key]
        public short cityID { get; set; }
        public string cityName { get; set; }
        public Nullable<float> longitude { get; set; }
        public Nullable<float> latitude { get; set; }
        public Nullable<float> totalArea { get; set; }
        public Nullable<float> populationCity { get; set; }
        public Nullable<short> provinceID { get; set; }
    
        public virtual PROVINCE PROVINCE { get; set; }
    }
}
