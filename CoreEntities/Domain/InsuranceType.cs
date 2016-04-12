
namespace Core.Domain
{
    using Core.Domain;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   public partial class InsuranceType : BaseEntity
    {
       [Key]
       public int InsuranceId { get; set; }
       public string InsuranceTypeName{get;set;}
       public Boolean Active { get; set; }
       public int ProviderId { get; set; }
    }
}
