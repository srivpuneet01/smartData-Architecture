using AppInterfaces.Infrastructure;
using AppInterfaces.Interfaces;
using AppInterfaces.Repository;
using Ninject.Modules;
using Ninject.Web.Common;
using RepositoryInfrastructure;
using RepositoryLayer;
using RepositoryLayer.Repositories;

namespace DependencyInjector
{
    public class DALModules : NinjectModule
    {
        public override void Load()
        {
            Bind<IEntitiesContext>().To<AzureDBContext>().InRequestScope();
            Bind<IAppUnitOfWork>().To<AppUnitOfWork>().InRequestScope();
            Bind<IActionAccessPermissionRepository>().To<ActionAccessPermissionRepository>().InRequestScope();
            Bind<ILogInfoRepository>().To<LogInfoRepository>().InRequestScope();
            Bind<IModulePermissionRepository>().To<ModulePermissionRepository>().InRequestScope();
            Bind<IModuleRepository>().To<ModuleRepository>().InRequestScope();
            Bind<IPagePermissionRepository>().To<PagePermissionRepository>().InRequestScope();
            Bind<IPatientRepository>().To<PatientRepository>().InRequestScope();
            Bind<IResetPasswordRepository>().To<ResetPasswordRepository>().InRequestScope();
            Bind<IRolesRepository>().To<RolesRepository>().InRequestScope();
            Bind<IScanMasterPagesRepository>().To<ScanMasterPagesRepository>().InRequestScope();
            Bind<ISchedulerRepository>().To<SchedulerRepository>().InRequestScope();
            Bind<IScreenPermissionRepository>().To<ScreenPermissionRepository>().InRequestScope();
            Bind<IScreenRepository>().To<ScreenRepository>().InRequestScope();
            Bind<IUserRepository>().To<UserRepository>().InRequestScope();
        }
    }
}