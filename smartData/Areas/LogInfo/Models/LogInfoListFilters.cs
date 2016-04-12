using Core.Domain;
using smartData.Filter;
using System.Collections.Generic;

namespace smartData
{
    public class LogInfoListFilters : GridFilter
    {
        public string ModuleName { get; set; }
        public string FieldName { get; set; }
        public string ModifiedBy { get; set; }
    }

}
