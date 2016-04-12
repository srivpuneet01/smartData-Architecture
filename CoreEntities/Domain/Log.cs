
namespace Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class Log : BaseEntity
    {
        [Key]
        public Guid LogId { get; set; }
        public int? ObjectId { get; set; }
        public int? ParentId { get; set; }
        public string TypeKey { get; set; }
        public string OperationKey { get; set; }
        public int? UserId { get; set; }
        public string Message { get; set; }
        public DateTime? Created { get; set; }
        public bool Processed { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public bool SendEmail { get; set; }
        public bool WriteAsFile { get; set; }
        public bool Representative { get; set; }
    }

}
