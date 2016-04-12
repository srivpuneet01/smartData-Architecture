using Core.Domain;
using Newtonsoft.Json.Linq;
using smartData.Filter;
using System.Collections.Generic;
using System.Web.Http;

namespace smartData
{
    public class LogInfoAPIController : ApiController, ILogInfoAPIController
    {
        ServiceLayer.Interfaces.ILogInfoService _loginfoService;        
        public LogInfoAPIController(ServiceLayer.Interfaces.ILogInfoService _LogInfoService)
        {
            _loginfoService = _LogInfoService;
        }      
        [HttpPost]
        [ActionName("GetLogList")]
        [AntiforgeryValidate]
        public dynamic GetLogList(JObject Obj)
        {
            LogInfoListFilters filter = Obj.ToObject<LogInfoListFilters>();
            int total;
            LogInfoReturnVal Lg = new LogInfoReturnVal();
            Lg.LogsData = _loginfoService.GetAllLogList(filter.limit, filter.offset, filter.order, filter.sort, filter.ModuleName, filter.FieldName,filter.ModifiedBy, out total);
            var result = new
            {
                total = total,
                rows = Lg.LogsData,
            };
            return result;
        }
    }
}
