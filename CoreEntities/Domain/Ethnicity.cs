

namespace Core.Domain
{
    using Core.Domain;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   public partial class Ethnicity : BaseEntity
    {
       public int EthnicityId { get; set; }
       public string EthnicityName { get; set; }
    }
}
