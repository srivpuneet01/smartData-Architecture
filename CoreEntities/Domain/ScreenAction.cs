
namespace Core.Domain
{
    using Core.Domain;
    using CoreEntities.CustomModels;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;
     [Table("ScreenAction")]
    public partial class ScreenAction : BaseEntity
    {
        [Key]
        public int ActionId { get; set; }
        [Required(ErrorMessage = "Screen Name is required.")]
        public string ActionName { get; set; }
        public int FK_screenId { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public int ActionType { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
