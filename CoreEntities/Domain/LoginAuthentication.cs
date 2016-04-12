namespace Core.Domain
{
    using Core.Domain;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("LoginAuthentication")]
   public partial class LoginAuthentication: BaseEntity
    {
        [Key]
        public int AuthenticationId { get; set; }
        public Nullable<DateTime> SessionTime { get; set; }
        public int ExpireTime { get; set; }
        public int UserId { get; set; }
        public Guid TokenId { get; set; }
        public bool Active { get; set; }
        public Nullable<DateTime> ActivityTime { get; set; }
    }
}

