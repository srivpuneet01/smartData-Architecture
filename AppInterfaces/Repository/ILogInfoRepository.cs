using Core.Domain;
using System.Collections.Generic;

namespace AppInterfaces.Interfaces
{
    public interface ILogInfoRepository
    {      
       List<LogInfo> GetAllLogList(int limit, int offset, string order, string sort, string ModuleName, string FieldName, string ModifiedBy, out int total);
       List<LogInfo> GetAllLogList(); 
   }
}
