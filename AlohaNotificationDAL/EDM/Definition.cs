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
    
    public partial class Definition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Definition()
        {
            this.Parameters = new HashSet<Parameter>();
        }
    
        public int Definition_Id { get; set; }
        public int EventMonitor_CLV_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int Target_Component_CLV_Id { get; set; }
        public int Target_SubComponent_CLV_Id { get; set; }
        public string DisplayTemplate { get; set; }
        public Nullable<bool> IsMonitored { get; set; }
        public bool IsRemoved { get; set; }
        public System.Guid Subscription_Id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Parameter> Parameters { get; set; }
    }
}
