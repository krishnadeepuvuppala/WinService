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
    
    public partial class UserQueue
    {
        public int UserQueue_Id { get; set; }
        public int Message_Id { get; set; }
        public int User_Id { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public Nullable<bool> IsHidden { get; set; }
        public Nullable<int> Createdby_Id { get; set; }
        public Nullable<long> CreatedOn { get; set; }
        public Nullable<int> LastEditedBy_Id { get; set; }
        public Nullable<long> LastEditedOn { get; set; }
        public bool IsRemoved { get; set; }
        public System.Guid Subscription_Id { get; set; }
    
        public virtual Message Message { get; set; }
    }
}
