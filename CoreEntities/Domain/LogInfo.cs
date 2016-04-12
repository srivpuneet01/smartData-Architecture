namespace Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class LogInfo : BaseEntity
    {         
        [Key]
        public string ModuleName { get; set; }
        public string FieldName { get; set; }             
        public string PreviousValue { get; set; }
        public string NewValue { get; set; }
        public string ModifiedBy { get; set; }
        public int UserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

}

