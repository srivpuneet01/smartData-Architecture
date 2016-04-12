namespace Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class ActionType: BaseEntity
    {
        public int ActionTypeId { get; set; }
        public string  TypeName { get; set; }
    }
}
