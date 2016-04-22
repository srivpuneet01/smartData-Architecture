using Core.Domain;
using CoreEntities.CustomModels;
using DTOLayer;
using Newtonsoft.Json.Linq;
using ServiceLayer.Interfaces;
using smartData.Areas.screens.Controllers;
using smartData.Filter;
using smartData.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ScreenAPIController : ApiController
    {
        IScreenService _screenService;
        public ScreenAPIController(IScreenService screenService)
        {
            _screenService = screenService;// _RolesService;

        }
        [HttpPost]
        [HandleAPIExceptionAttribute]
        public ResponseModel CreateScreen(Screen screen)
        {
            return _screenService.Createscreen(screen);
        }

        // add ScreenAction
        [HttpPost]
        [AllowAnonymous]
        public ResponseModel AddScreen(ScreenAction screenAction)
        {
            ResponseModel _result = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    // role.Active = true;
                    _result = _screenService.AddScreenAction(screenAction);
                }
                return _result;

            }
            catch (Exception)
            {
                return new ResponseModel { ResponseStatus = false, ResponseText = "There are some error please try again." };
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IEnumerable<Screen> GetScreen(screenListFilters filter)
        {
            int total;
            screenReturnVal re = new screenReturnVal();
            filter.limit = 10000;
            filter.order = "ASC";
            filter.sort = "ASC";
            filter.screenName = filter.screenName == null ? "" : filter.screenName;
            re.data = _screenService.GetAllscreen(filter.limit, filter.offset, filter.order, filter.sort, filter.screenName, out total);
            return re.data;
        }

        [HttpPost]
        [AllowAnonymous]
        public IEnumerable<ScreenAction> GetScreenAction(screenActionListFilters filter)
        {
            int total;
            screenActionReturnVal re = new screenActionReturnVal();
            re.data = _screenService.GetAllScreenAction(filter.limit, filter.offset, filter.order, filter.sort, filter.FK_screenId, out total);
            return re.data;
        }


        [HttpPost]
        [AllowAnonymous]
        public ResponseModel EditScreen(Screen screen)
        {

            ResponseModel oResponseModel = new ResponseModel();
            try
            {
                _screenService.Editscreen(screen);
                oResponseModel.ResponseText = "Screen Updated Sucessfully";
                oResponseModel.ResponseStatus = true;
            }
            catch (Exception)
            {
                oResponseModel.ResponseText = "Error! in updating Screen";
                oResponseModel.ResponseStatus = false;
            }

            return oResponseModel;

        }

        [HttpPost]
        [AllowAnonymous]
        public ResponseModel EditScreenAction(ScreenAction screenAction)
        {
            ResponseModel oResponseModel = new ResponseModel();
            try
            {
                _screenService.EditScreenAction(screenAction);
                oResponseModel.ResponseText = "Screen Updated Sucessfully";
                oResponseModel.ResponseStatus = true;
            }
            catch (Exception ex)
            {
                oResponseModel.ResponseText = "Error! in updating Screen";
                oResponseModel.ResponseStatus = false;
            }

            return oResponseModel;



        }
        [HttpGet]
        [AllowAnonymous]
        public ResponseModel Deletescreen(int id)
        {
            ResponseModel oResponseModel = new ResponseModel();
            if (_screenService.Deletescreen(id) == true)
            {
                oResponseModel.ResponseText = "Deleted Sucessfully";
                oResponseModel.ResponseStatus = true;
            }
            else
            {
                oResponseModel.ResponseText = "Error";
                oResponseModel.ResponseStatus = false;
            }
            return oResponseModel;

        }
        [HttpPost]
        [ActionName("DeleteScreenAction")]
        [AntiforgeryValidate]
        public bool DeleteScreen(int id)
        {
            return _screenService.DeleteScreenAction(id);
        }

        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<SelectedListDTO> GetAction()
        {
            IEnumerable<ModelType> actionTypes = Enum.GetValues(typeof(ModelType))
                                                       .Cast<ModelType>();
            var actionlist = from action in actionTypes
                             select new SelectedListDTO { Text = action.ToString(), Value = ((int)action).ToString(), };
            return actionlist.ToList();
        }

        [HttpPost]
        [AllowAnonymous]
        public ResponseModel AddScreenAction(ScreenAction screenAction)
        {
            try
            {
                return _screenService.AddScreenAction(screenAction);
            }
            catch (Exception)
            {
                return new ResponseModel { ResponseStatus = false, ResponseText = "There are some error in insert Action!!" };

            }
        }
    }
}