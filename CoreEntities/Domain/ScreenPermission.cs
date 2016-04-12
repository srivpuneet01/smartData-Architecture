
namespace Core.Domain
{
    using CoreEntities.CustomModels;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;
    [Table("ScreenPermission")]
    public partial class ScreenPermission : BaseEntity
    {
        [Key]
       
        public int ScreenPermissionID { get; set; }
       
        public int ScreenId { get; set; }
        
        public int ActionId { get; set; }
      
        public string ActionName { get; set; }
      
        public Nullable <int> CreatedBy { get; set; }
      
        public Nullable <System.DateTime> CreatedDate { get; set; }

        public Nullable <bool> IsDeleted { get; set; }
       
        public Nullable<int> DeletedBy { get; set; }
      
        public Nullable<System.DateTime> DeletedDate { get; set; }
        [NotMapped]
        public string ScreenName { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> ModuleList { get; set; }
        [NotMapped]
       // public List<Roles> RoleList { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
        [NotMapped]
        public List<Screen> SceenList { get; set; }
        
        public int RoleId { get; set; }
        
        public int ModuleID { get; set; }
         [NotMapped]
        public int ActionType { get; set; }
         public Nullable<int> ModifiedBy { get; set; }
         public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
