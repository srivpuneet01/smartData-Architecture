using AppInterfaces.Interfaces;
using Core.Domain;
using CoreEntities.CustomModels;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;

namespace ServiceLayer.Services
{
    public class ScreenService : IScreenService
    {
        #region Global Variables
        IScreenRepository _screenRepository = null;
        #endregion

        #region constructor
        public ScreenService(IScreenRepository screenRepository)
        {

            _screenRepository = screenRepository;// new RepositoryLayer.Repositories.ScreenRepository();//_RolesRepository;
        }
        #endregion

        #region  Public Methods

        public ResponseModel Createscreen(Screen screen)
        {
            return _screenRepository.Createscreen(screen);

        }
        // add screenAction
        public ResponseModel AddScreenAction(ScreenAction screenAction)
        {
            return _screenRepository.AddScreenAction(screenAction);

        }

        public List<Screen> GetAllscreen(int limit, int offset, string order, string sort, string screenName, out int total)
        {
            return _screenRepository.GetAllscreen(limit, offset, order, sort, screenName, out total);
        }
        public String Editscreen(Screen screen)
        {

            screen.CreatedDate = DateTime.Now;
            return _screenRepository.Editscreen(screen);
        }

        public bool Deletescreen(int id)
        {
            return _screenRepository.Deletescreen(id);
        }
        public List<ScreenAction> GetAllScreenAction(int limit, int offset, string order, string sort, int fk_screenId, out int total)
        {
            return _screenRepository.GetAllScreenAction(limit, offset, order, sort, fk_screenId, out total);
        }
        public Screen GetDropDown()
        {
            return _screenRepository.GetDropDown();
        }
        public List<ScreenAction> GetAllScreenActionByScreeiID(int fk_screenId)
        {
            return _screenRepository.GetAllScreenActionByScreeiID(fk_screenId);
        }

        public void EditScreenAction(ScreenAction screenAction)
        {

            screenAction.CreatedDate = DateTime.Now;
            _screenRepository.EditScreenAction(screenAction);
        }
        public bool DeleteScreenAction(int id)
        {
            return _screenRepository.DeleteScreenAction(id);
        }
        #endregion
    }
}
