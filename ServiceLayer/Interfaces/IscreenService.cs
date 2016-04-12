using Core.Domain;
using CoreEntities.CustomModels;
using System.Collections.Generic;

namespace ServiceLayer.Interfaces
{
    public interface IScreenService
    {
        ResponseModel Createscreen(Screen screen);
        List<Screen> GetAllscreen(int limit, int offset, string order, string sort, string screenName, out int total);
        string Editscreen(Screen screen);
        bool Deletescreen(int id);
        ResponseModel AddScreenAction(ScreenAction screenaction);
        List<ScreenAction> GetAllScreenAction(int limit, int offset, string order, string sort, int FK_screenId, out int total);
        Screen GetDropDown();
        List<ScreenAction> GetAllScreenActionByScreeiID(int FK_screenId);
        void EditScreenAction(ScreenAction screenaction);
        bool DeleteScreenAction(int id);

    }
}
