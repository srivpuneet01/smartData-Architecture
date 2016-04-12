using Core.Domain;
using CoreEntities.CustomModels;
using System.Collections.Generic;

namespace AppInterfaces.Interfaces
{
    public interface IScreenRepository
    {
        ResponseModel Createscreen(Screen screen);
        List<Screen> GetAllscreen(int limit, int offset, string order, string sort, string screenName, out int total);
        string Editscreen(Screen screen);
        bool Deletescreen(int id);
        ResponseModel AddScreenAction(ScreenAction screenAction);
        List<ScreenAction> GetAllScreenAction(int limit, int offset, string order, string sort, int fk_screenId, out int total);
        Screen GetDropDown();
        List<ScreenAction> GetAllScreenActionByScreeiID(int fk_screenId);
        void EditScreenAction(ScreenAction screenAction);
        bool DeleteScreenAction(int id);
    }
}
