using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CoreEntities.CustomModels;
namespace CoreEntities.Domain
{
   public class ActionModel
    {
        public ActionModel()
        {
            ModuleIdList = new List<SelectListItem>();
        }
      //  public ModelType something { get; set; }
        [Display(Name = "Actions")]
        public int ModuleId { get; set; }
        public IEnumerable<SelectListItem> ModuleIdList { get; set; } 
    }
}
