namespace Core.Domain
{
    using CoreEntities.CustomModels;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;
    [Table("screen")]
    public partial class Screen : BaseEntity
    {
        //[Key]
        public int screenId { get; set; }
        [Required(ErrorMessage = "Screen Name is required.")]
        [StringLength(50, ErrorMessage = "Screen Name cannot be longer than 50 characters.")]
        public string ScreenName { get; set; }
     
        public int ModuleId { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        //Add new
      //  public int ModuleId { get; set; }
        public IEnumerable<SelectListItem> ModuleIdList { get; set; }
        public ModelType ModuleName;
        public IEnumerable<SelectListItem> ModuleList { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
