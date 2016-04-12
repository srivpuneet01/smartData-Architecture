namespace Core.Domain
{
    using Core.Domain;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class Program : BaseEntity
    {
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }
    }
}
