using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using ServiceLayer.Interfaces;
using ServiceLayer.Services;
using System;
using System.Web;

namespace DependencyInjector
{
    public class ServiceLayerModules : NinjectModule
    {
        public override void Load()
        {
            Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            Bind<IEmailService>().To<EmailService>();
            Bind<IScreenPermissionService>().To<ScreenPermissionService>();
            Bind<IResetPasswordService>().To<ResetPasswordService>();
        }
    }
}