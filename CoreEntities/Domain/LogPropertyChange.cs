
namespace Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class LogPropertyChange : BaseEntity
    {
        [Key]
        public System.Guid LogPropertyChangeId { get; set; }
        public System.Guid LogId { get; set; }
        public string PropertyKey { get; set; }
        public string PreviousValue { get; set; }
        public string NewValue { get; set; }
        public string EncryptionKey { get; set; }

        public virtual Log Log { get; set; }
    }

}
