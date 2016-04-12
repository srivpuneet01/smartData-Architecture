using Newtonsoft.Json.Linq;

namespace smartData
{
    public interface ILogInfoAPIController
    {        
        dynamic GetLogList(JObject obj);     
    }
}
