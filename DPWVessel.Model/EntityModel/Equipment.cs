//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DPWVessel.Model.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Equipment
    {
        public int Id { get; set; }
        public Nullable<int> EquipmentTypeId { get; set; }
        public string Name { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    
        public virtual EquipmentType EquipmentType { get; set; }
    }
}
