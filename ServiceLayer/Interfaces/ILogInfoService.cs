using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface ILogInfoService
    {
        List<LogInfo> GetAllLogList();
        List<LogInfo> GetAllLogList(int limit, int offset, string order, string sort, string ModuleName, string FieldName, string ModifiedBy, out int total);      

    }
}
