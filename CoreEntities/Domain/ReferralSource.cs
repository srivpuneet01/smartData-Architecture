

namespace Core.Domain
{
    using Core.Domain;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
  public partial  class ReferralSource : BaseEntity
    {
      public int ReferralSourceId { get; set; }
      public string ReferralSourceName{get;set;}
    }
}
