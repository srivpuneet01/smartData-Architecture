namespace Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class LogProperty : BaseEntity
    {
        [Key, Column(Order = 0)]
        public string TypeKey { get; set; }
        [Key, Column(Order = 1)]
        public string Key { get; set; }
        public string Name { get; set; }
        public bool LogValues { get; set; }
    }
}
