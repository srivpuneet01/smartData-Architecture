[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(smartData.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(smartData.App_Start.NinjectWebCommon), "Stop")]

namespace smartData.App_Start
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Modules;
    using System.Collections.Generic;
    using System.Web.Http;
    using ServiceLayer.Interfaces;
    using ServiceLayer.Services;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        //private static NinjectModule[] GetAllNinjectModules()
        //{
        //    return new NinjectModule[]
        //               {
        //                   new DALModules(),
        //                   new ServiceLayerModules()
        //               };
        //}

        
        //protected override IKernel CreateKernel()
        //{
        //    kernel = new StandardKernel(GetAllNinjectModules());
        //    return kernel;
        //}
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
               

                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

                RegisterServices(kernel);
                var modules = new List<INinjectModule>
                {
                    new ServiceLayer.DependencyResolver()
                };
                kernel.Load(modules);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            kernel.Bind<IScreenPermissionService>().To<ScreenPermissionService>();
            kernel.Bind<IEmailService>().To<EmailService>();
            kernel.Bind<IUsersAPIController>().To<UsersAPIController>();
            kernel.Bind<IUserService>().To<ServiceLayer.Services.UserService>();            
            kernel.Bind<IActionAccessPermissionService>().To<ServiceLayer.Services.ActionAccessPermissionService>();
            kernel.Bind<IProgramService>().To<ServiceLayer.Services.ProgramService>();            
            kernel.Bind<IModuleService>().To<ModuleService>();
            
            kernel.Bind<IModulePermissionService>().To<ModulePermissionService>();            
            kernel.Bind<IRolesService>().To<RolesService>();
            
            kernel.Bind<IScreenService>().To<ScreenService>();
            kernel.Bind<ILogInfoAPIController>().To<LogInfoAPIController>();
            kernel.Bind<ILogInfoService>().To<ServiceLayer.Services.LogInfoService>();
            kernel.Bind<IPagePermissionAPIController>().To<PagePermissionAPIController>();
            kernel.Bind<IPagePermissionService>().To<ServiceLayer.Services.PagePermissionService>();
            kernel.Bind<IMessageFormatter>().To<ServiceLayer.Email.HtmlMessageFormatter>();
            kernel.Bind<IMessageSender>().To<ServiceLayer.Email.EmailMessageSender>();
            kernel.Bind<IConfig>().To<ServiceLayer.Email.Config>();
            


        }
    }
}
