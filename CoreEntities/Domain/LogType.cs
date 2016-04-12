
namespace Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class LogTypes : BaseEntity
    {
        [Key]
        public string Key { get; set; }
        public string Name { get; set; }
        public string IdName { get; set; }
        public string ClientIdName { get; set; }
        public string ParentIdName { get; set; }
    }

}
