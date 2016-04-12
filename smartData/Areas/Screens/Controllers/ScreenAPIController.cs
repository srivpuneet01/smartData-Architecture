using Core.Domain;
using CoreEntities.CustomModels;
using Newtonsoft.Json.Linq;
using ServiceLayer.Interfaces;
using smartData.Areas.screens.Controllers;
using smartData.Filter;
using smartData.Infrastructure;
using System.Collections.Generic;
using System.Web.Http;

namespace smartData
{
    public class screenListFilters : GridFilter
    {
        public string screenName { get; set; }

    }
    public class screenActionListFilters : GridFilter
    {
        public int FK_screenId { get; set; }

    }

    public class screenReturnVal
    {
        public IEnumerable<Screen> data;
        public int total;
    }
    public class screenActionReturnVal
    {
        public IEnumerable<ScreenAction> data;
        public int total;
    }
    [HandleException]
    public class ScreenAPIController : ApiController, IScreenAPIController
    {
        IScreenService _screenService;
        public ScreenAPIController(IScreenService screenService)
        {
            _screenService = screenService;// _RolesService;
        }
        [HttpPost]
        [HandleAPIExceptionAttribute]
        public ResponseModel Createscreen(Screen screen)
        {
            return _screenService.Createscreen(screen);
        }
        // add ScreenAction
        [HttpPost]
        [HandleAPIExceptionAttribute]
        [ActionName("AddScreenAction")]
        public ResponseModel AddScreenAction(ScreenAction screenAction)
        {
            return _screenService.AddScreenAction(screenAction);
        }

        [HttpPost]
        [ActionName("GetScreens")]
        [AntiforgeryValidate]
        public dynamic GetScreens(JObject Obj)
        {
            screenListFilters filter = Obj.ToObject<screenListFilters>();
            int total;
            screenReturnVal re = new screenReturnVal();
            re.data = _screenService.GetAllscreen(filter.limit, filter.offset, filter.order, filter.sort, filter.screenName, out total);

            var result = new
            {
                total = total,
                rows = re.data,
            };
            return result;
        }

        [HttpPost]
        [ActionName("GetScreenAction")]
        [AntiforgeryValidate]
        public dynamic GetScreenAction(JObject Obj)
        {
            screenActionListFilters filter = Obj.ToObject<screenActionListFilters>();
            int total;
            screenActionReturnVal re = new screenActionReturnVal();
            re.data = _screenService.GetAllScreenAction(filter.limit, filter.offset, filter.order, filter.sort, filter.FK_screenId, out total);

            var result = new
            {
                total = total,
                rows = re.data,
            };
            return result;
        }


        [HttpPost]
        [ActionName("Editscreen")]
        [AntiforgeryValidate]
        public string Editscreen(Screen screen)
        {
            return _screenService.Editscreen(screen);
        }

        [HttpPost]
        [ActionName("EditScreenAction")]
        [AntiforgeryValidate]
        public void EditScreenAction(ScreenAction screenAction)
        {
            _screenService.EditScreenAction(screenAction);
        }


        [HttpPost]
        [ActionName("Deletescreen")]
        [AntiforgeryValidate]
        public bool Deletescreen(int id)
        {
            return _screenService.Deletescreen(id);
        }
        [HttpPost]
        [ActionName("DeleteScreenAction")]
        [AntiforgeryValidate]
        public bool DeleteScreenAction(int id)
        {
            return _screenService.DeleteScreenAction(id);
        }
    }
}