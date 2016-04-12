namespace Core.Domain
{
    using Core.Domain;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("Module")]
    public partial class Module: BaseEntity
    {
        public int ModuleID { get; set; }
        [Required(ErrorMessage = "Module name is required.")]
        public string ModuleName { get; set; }
        //---------------Add New Property------------
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        //--------------------End-----------------------------------
    }
}
