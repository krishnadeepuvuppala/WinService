//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AlohaNotificationDAL.EDM
{
    using System;
    using System.Collections.Generic;
    
    public partial class ParameterValue
    {
        public int ParameterValue_Id { get; set; }
        public int Parameter_Id { get; set; }
        public string ParameterValue1 { get; set; }
        public bool IsRemoved { get; set; }
        public System.Guid Subscription_Id { get; set; }
    
        public virtual Parameter Parameter { get; set; }
    }
}