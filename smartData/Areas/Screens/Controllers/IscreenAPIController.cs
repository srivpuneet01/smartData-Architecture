using Core.Domain;
using CoreEntities.CustomModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartData
{
    public interface IScreenAPIController
    {

        ResponseModel Createscreen(Screen screen);
        string Editscreen(Screen screen);
        bool Deletescreen(int id);
        ResponseModel AddScreenAction(ScreenAction screenAction);
        void EditScreenAction(ScreenAction screenAction);
        bool DeleteScreenAction(int id);
    }
}
