using AppInterfaces.Infrastructure;
using AppInterfaces.Interfaces;
using AppInterfaces.Repository;
using Core.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RepositoryLayer.Repositories
{
    public class LogInfoRepository : BaseRepository<LogInfo>, ILogInfoRepository
    {
        #region ctor
        public LogInfoRepository(IAppUnitOfWork uow)
            : base(uow)
        {

        }
        #endregion

        #region  Public Methods

        public List<LogInfo> GetAllLogList(int limit, int offset, string order, string sort, string ModuleName, string FieldName, string ModifiedBy, out int total)
        {
            string param = null;
            Context.ExecuteStoredProcedureList<LogInfo>("Getlogs", param);//TODO

            var data = Context.Set<LogInfo>().AsQueryable();
            //Order by
            GetSortedData(ref data, sort + order.ToUpper());
            total = data.Count();
            return data.Skip(offset).Take(limit).ToList();
        }
        public List<LogInfo> GetAllLogList()
        {
            var data = Context.Set<LogInfo>().AsQueryable();
            return data.ToList();
        }
        public void GetSortedData(ref IQueryable<LogInfo> obj, string Case)
        {
            switch (Case)
            {
                case "ModuleNameASC":
                    obj = obj.OrderBy(x => x.ModuleName.ToLower());
                    break;
                case "FieldNameASC":
                    obj = obj.OrderBy(x => x.FieldName.ToLower());
                    break;
                case "ModifiedByASC":
                    obj = obj.OrderBy(x => x.UserId);
                    break;
                case "ModuleNamDESC":
                    obj = obj.OrderByDescending(x => x.UserId);
                    break;
                case "FieldNameDESC":
                    obj = obj.OrderByDescending(x => x.FieldName.ToLower());
                    break;
                case "ModifiedByDESC":
                    obj = obj.OrderByDescending(x => x.UserId);
                    break;
                default:
                    obj = obj.OrderBy(x => x.UserId);
                    break;
            }
        }
        #endregion
    }
}
