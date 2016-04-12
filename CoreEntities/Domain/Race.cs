using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
  public partial  class Race :BaseEntity
    {
      public int RaceId { get; set; }
      public string RaceName { get; set; }
    }
}
