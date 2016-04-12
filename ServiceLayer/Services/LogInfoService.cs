using AppInterfaces.Interfaces;
using AppInterfaces.Repository;
using Core.Domain;
using System.Collections.Generic;

namespace ServiceLayer.Services
{
    public class LogInfoService : Interfaces.ILogInfoService
    {
        #region Global Variables

        ILogInfoRepository _logInfoRepository = null;
        #endregion

        #region  Public Methods

        public LogInfoService(ILogInfoRepository logInfoRepository)
        {
            _logInfoRepository = logInfoRepository;
        }

        public List<LogInfo> GetAllLogList()
        {
            return _logInfoRepository.GetAllLogList();
        }
        public List<LogInfo> GetAllLogList(int limit, int offset, string order, string sort, string moduleName, string fieldName, string modifiedBy, out int total)
        {
            return _logInfoRepository.GetAllLogList(limit, offset, order, sort, moduleName, fieldName, modifiedBy, out total);
        }
        #endregion
    }
}
