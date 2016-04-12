using Core.Domain;
using AppInterfaces.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer.Services
{
    public class ProgramService : IProgramService
    {
        #region Global Variables

        IProgramRepository _programRepository = null;
        #endregion

        #region constructor
      
        public ProgramService(IProgramRepository programRepository)
        {
            _programRepository = programRepository;
        }
        #endregion

        #region  Public Methods
        #endregion
    }
}
