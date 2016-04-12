using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RepositoryLayer
{
    public class DependencyResolver : NinjectModule
    {
        public override void Load()
        {
            //Bind<RepositoryLayer.Interfaces.IUserRepository>().To<RepositoryLayer.Repositories.UserRepository>();
            //Bind<RepositoryLayer.Interfaces.IPatientRepository>().To<RepositoryLayer.Repositories.PatientRepository>();
            //Bind<RepositoryLayer.Interfaces.IProgramRepository>().To<RepositoryLayer.Repositories.ProgramRepository>();
            //Bind<RepositoryLayer.Interfaces.ISchedulerRepository>().To<RepositoryLayer.Repositories.SchedulerRepository>();
            //Bind<RepositoryLayer.Interfaces.ILogInfoRepository>().To<RepositoryLayer.Repositories.LogInfoRepository>();

        }
    }
}
