namespace Core.Domain
{
    using Core.Domain;
    using CoreEntities.CustomModels;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;
   public partial  class ScreenPermissionList: BaseEntity
    {
       public string ScreenName { get; set; }

       public string ActionName { get; set; }
       public int ScreenId { get; set; }
        public int ActionType { get; set; }
        public int ActionId { get; set; }
        public string IsAllowPermission { get; set; }

    }
}
