using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
  public partial  class ServiceRequest:BaseEntity
    {
      public int ServiceRequestId { get; set; }
      public string ServiceRequested { get; set; }
      public int ProviderId { get; set; }
      public Boolean Active { get; set; }
    }
}
