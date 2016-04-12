namespace Core.Domain
{
    using Core.Domain;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    [Table("SecUserPasswordHistory")]
    public class SecUserPasswordHistory
    {
        public int SecUserPasswordHistoryID { get; set; }
        public int SecUserID { get; set; }
        public byte[]   PasswordHash256 { get; set; }
        public Nullable<bool> DeleteFlag { get; set; }
        public string RowVersion { get; set; }
    }
}
