using AppInterfaces.Infrastructure;
using AppInterfaces.Interfaces;
using Core.Domain;
using CoreEntities.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RepositoryLayer.Repositories
{
    public class ScreenRepository : BaseRepository<Screen>, IScreenRepository
    {
        #region Initialize
        public ScreenRepository(IAppUnitOfWork uow)
            : base(uow)
        {
        }
        #endregion

        #region  Public Methods

        public ResponseModel Createscreen(Screen screen)
        {
            ResponseModel result = new ResponseModel();

            if (!Any(x => !String.IsNullOrWhiteSpace(x.ScreenName) && x.ScreenName.ToLower() == screen.ScreenName.ToLower() && x.IsDeleted == false))
            {
                screen.CreatedBy = screen.CreatedBy;
                screen.CreatedDate = DateTime.UtcNow;
                screen.IsDeleted = false;
                Add(screen);
                SaveChanges();

                result.ResponseStatus = true;
                result.ResponseText = "Screen created successfully.";
            }
            else
            {
                result.ResponseStatus = false;
                result.ResponseText = "Sorry Duplicate Records exists.";
            }
            return result;
        }

        public ResponseModel AddScreenAction(ScreenAction screenAction)
        {
            ResponseModel result = new ResponseModel();

            string action = ((ModelType)screenAction.ActionType).ToString();
            int value;
            if (int.TryParse(action, out value))
            {
                result.ResponseStatus = false;
                result.ResponseText = "Sorry, Action Type has some error.";
                return result;
            }

            if (!Context.Set<ScreenAction>().Any(x => x.FK_screenId == screenAction.FK_screenId && x.IsDeleted == false && x.ActionType == screenAction.ActionType))
            {
                screenAction.CreatedBy = 0;
                screenAction.CreatedDate = DateTime.UtcNow;
                screenAction.IsDeleted = false;
                Context.Set<ScreenAction>().Add(screenAction);

                SaveChanges();
                result.ResponseStatus = true;
                result.ResponseText = "ScreenAction created successfully.";
            }
            else
            {
                result.ResponseStatus = false;
                result.ResponseText = "Sorry Duplicate Records exists.";
            }
            return result;

        }
        public List<Screen> GetAllscreen(int limit, int offset, string order, string sort, string screenName, out int total)
        {
            var data = GetAll(x => !x.IsDeleted).AsQueryable();//Where(x => x.Active && !x.IsDeleted);
            if (!string.IsNullOrEmpty(screenName))
            {
                data = data.Where(x => x.ScreenName.StartsWith(screenName));
            }
            GetSortedData(ref data, sort + order.ToUpper());

            total = data.Count();
            return data.Skip(offset).Take(limit).OrderByDescending(x => x.screenId).ToList();
        }

        public List<ScreenAction> GetAllScreenAction(int limit, int offset, string order, string sort, int FK_screenId, out int total)
        {
            var data = Context.Set<ScreenAction>().Where(x => !x.IsDeleted);//Where(x => x.Active && !x.IsDeleted);

            GetSortedDataForAction(ref data, sort + order.ToUpper());

            total = data.Count();
            return data.Skip(offset).Take(limit).ToList();
        }

        public List<ScreenAction> GetAllScreenActionByScreeiID(int fk_screenId)
        {
            return Context.Set<ScreenAction>().Where(x => !x.IsDeleted && x.FK_screenId == fk_screenId).ToList();//Where(x => x.Active && !x.IsDeleted);
        }

        public void GetSortedDataForAction(ref IQueryable<ScreenAction> screenActionQuery, string sortingOrder)
        {
            switch (sortingOrder)
            {
                case "ActionNameASC":
                    screenActionQuery = screenActionQuery.OrderBy(x => x.ActionName.ToLower());
                    break;
                case "ActionNameDESC":
                    screenActionQuery = screenActionQuery.OrderByDescending(x => x.ActionName.ToLower());
                    break;
                case "ActionIdESC":
                    screenActionQuery = screenActionQuery.OrderByDescending(x => x.ActionId);
                    break;
                case "ActionIdASC":
                    screenActionQuery = screenActionQuery.OrderBy(x => x.ActionId);
                    break;
                default:
                    screenActionQuery = screenActionQuery.OrderBy(x => x.ActionId);
                    break;
            }
        }

        public void GetSortedData(ref IQueryable<Screen> screenQuery, string sortingOrder)
        {
            switch (sortingOrder)
            {
                case "ScreenNameASC":
                    screenQuery = screenQuery.OrderBy(x => x.ScreenName.ToLower());
                    break;
                case "ScreenNameDESC":
                    screenQuery = screenQuery.OrderByDescending(x => x.ScreenName.ToLower());
                    break;
                case "screenIdESC":
                    screenQuery = screenQuery.OrderByDescending(x => x.screenId);
                    break;
                case "screenIdASC":
                    screenQuery = screenQuery.OrderBy(x => x.screenId);
                    break;
                default:
                    screenQuery = screenQuery.OrderByDescending(x => x.screenId);
                    break;
            }
        }
        public String Editscreen(Screen screen)
        {
            if (Context.Set<Screen>().Any(x => x.ScreenName.ToLower() == screen.ScreenName.ToLower() && x.IsDeleted == false && x.screenId != screen.screenId))
            {
                return "false";
            }
            else
            {
                var objscreen = Context.Set<Screen>().FirstOrDefault(x => x.screenId == screen.screenId);
                objscreen.ScreenName = screen.ScreenName;
                objscreen.ModuleId = screen.ModuleId;
                objscreen.ModifiedBy = screen.ModifiedBy;
                objscreen.ModifiedDate = DateTime.UtcNow;
                objscreen.CreatedDate = screen.CreatedDate;
                SaveChanges();
                return "true";
            }
        }
        public bool Deletescreen(int id)
        {
            Screen screen = Get(x => x.screenId == id && !x.IsDeleted);
            screen.IsDeleted = true;
            screen.DeletedBy = 1;
            screen.DeletedDate = DateTime.UtcNow;
            SaveChanges();
            return true;
        }
        public Screen GetDropDown()
        {
            IEnumerable<SelectListItem> ModuleList = (from x in Context.Set<Module>()
                                                      where x.IsDeleted != true
                                                      select new SelectListItem() { Text = x.ModuleName.ToString(), Value = x.ModuleID.ToString() }).ToList();
            Screen screenObject = new Screen();
            screenObject.ModuleList = ModuleList;
            return screenObject;
        }
        public void EditScreenAction(ScreenAction screenAction)
        {
            var screenactionObject = Context.Set<ScreenAction>().FirstOrDefault(x => x.ActionId == screenAction.ActionId);
            screenactionObject.ActionName = screenAction.ActionName;
            screenactionObject.ActionType = screenAction.ActionType;
            screenactionObject.ModifiedBy = screenAction.ModifiedBy;
            screenactionObject.ModifiedDate = DateTime.UtcNow;
            screenactionObject.CreatedDate = screenAction.CreatedDate;
            SaveChanges();
        }

        public bool DeleteScreenAction(int id)
        {
            ScreenAction screenaction = Context.Set<ScreenAction>().FirstOrDefault(x => x.ActionId == id && !x.IsDeleted);
            screenaction.IsDeleted = true;
            screenaction.DeletedBy = 1;
            screenaction.DeletedDate = DateTime.UtcNow;
            SaveChanges();
            return true;
        }
        #endregion
    }
}
