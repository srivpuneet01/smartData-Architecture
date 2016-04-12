using Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Entity;
using CoreEntities.CustomModels;
using AppInterfaces.Infrastructure;
using AppInterfaces.Interfaces;

namespace RepositoryLayer.Repositories
{
    public class ModuleRepository : BaseRepository<Module>, IModuleRepository
    {
        #region Initialize

        public ModuleRepository(IAppUnitOfWork uow)
            : base(uow)
        {
        }
        #endregion

        public ResponseModel CreateModule(Module module)
        {
            ResponseModel result = new ResponseModel();

            if (!Context.Set<Module>().Any(x => x.ModuleName.ToLower() == module.ModuleName.ToLower()
            && !x.IsDeleted))
            {
                module.CreatedBy = 0;
                module.CreatedDate = DateTime.UtcNow;
                module.IsDeleted = false;


                Context.Set<Module>().Add(module);
                Context.SaveChanges();

                result.ResponseStatus = true;
                result.ResponseText = "Module created successfully.";
            }
            else
            {
                result.ResponseStatus = false;
                result.ResponseText = "Sorry Duplicate Records exists.";
            }

            return result;
        }


        public List<Module> GetAllModules(int limit, int offset, string order, string sort, string moduleName, out int total)
        {
           var modulesSet = Context.Set<Module>().Where(x => x.IsDeleted == false).AsQueryable();
            if (!string.IsNullOrEmpty(moduleName))
            {
                modulesSet = modulesSet.Where(x => x.ModuleName.StartsWith(moduleName));
            }

            GetSortedData(ref modulesSet, sort + order.ToUpper());

            total = modulesSet.Count();
            return modulesSet.Skip(offset).Take(limit).ToList();
        }
        
        public void GetSortedData(ref IQueryable<Module> moduleQuery, string sortingOrder)
        {
            switch (sortingOrder)
            {
                case "ModuleNameASC":
                    moduleQuery = moduleQuery.OrderBy(x => x.ModuleName.ToLower());
                    break;

                case "ModuleNameDESC":
                    moduleQuery = moduleQuery.OrderByDescending(x => x.ModuleName.ToLower());
                    break;

                case "UserIdDESC":
                    moduleQuery = moduleQuery.OrderByDescending(x => x.ModuleID);
                    break;
                case "UserIdASC":
                    moduleQuery = moduleQuery.OrderBy(x => x.ModuleID);
                    break;
                default:
                    moduleQuery = moduleQuery.OrderByDescending(x => x.ModuleID);
                    break;
            }
        }

        public void EditModule(Module module)
        {
            var existedModule = Get(x => x.ModuleID == module.ModuleID);

            if (existedModule == null) return;

            existedModule.ModuleName = module.ModuleName;
            existedModule.ModifiedBy = module.ModifiedBy;
            existedModule.ModifiedDate = DateTime.UtcNow;
            Context.SaveChanges();
        }
        public bool DeleteModule(int id)
        {
            if (!Context.Set<Screen>().Any(x => x.ModuleId == id && x.IsDeleted == false))
            {
                // Roles role = uow.Repository<Roles>().AsQuerable().FirstOrDefault(x => x.RoleId == id && !x.IsDeleted && x.Active);
                Module module = Get(x => x.ModuleID == id && !x.IsDeleted);
                //  uow.Repository<Module>().Attach(Module);
                module.IsDeleted = true;
                module.DeletedBy = 1;
                module.DeletedDate = DateTime.UtcNow;
                Context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
