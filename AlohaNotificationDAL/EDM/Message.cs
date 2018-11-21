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
    
    public partial class Message
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Message()
        {
            this.UserQueues = new HashSet<UserQueue>();
        }
    
        public int Message_Id { get; set; }
        public string QueueDisplayMessage { get; set; }
        public Nullable<long> CreatedTime { get; set; }
        public Nullable<int> Target_Id { get; set; }
        public Nullable<int> Createdby_Id { get; set; }
        public Nullable<long> CreatedOn { get; set; }
        public Nullable<int> LastEditedBy_Id { get; set; }
        public Nullable<long> LastEditedOn { get; set; }
        public Nullable<bool> IsRemoved { get; set; }
        public Nullable<System.Guid> Subscription_Id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserQueue> UserQueues { get; set; }
    }
}
