namespace Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
   public partial  class Roles:BaseEntity
    {   
         [Key]
        public int RoleId { get; set; }
       
        //(ErrorMessage = "Role Name is required.")]  
       [Required(ErrorMessage = "Role Name is required.")]
        public string RoleName { get; set; }
        
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
      
    // This is a new property
    [NotMapped]
       public string ActiveDisplay
    {
        get
        {
            // If the value given to the side is negative,
            // then set it to 0
            if (Active ==false)
                return "No";
            else
                return "Yes";
        }
    }

    
}
}

