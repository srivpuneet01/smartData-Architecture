using Core.Domain;
using AppInterfaces.Interfaces;
using AppInterfaces.Infrastructure;

namespace RepositoryLayer.Repositories
{
    public class ProgramRepository : BaseRepository<Program>, IProgramRepository
    {
        #region Initialize
        public ProgramRepository(IAppUnitOfWork uow)
             : base(uow)
        {

        }

        #endregion

        #region  Public Methods
        #endregion
    }

}
