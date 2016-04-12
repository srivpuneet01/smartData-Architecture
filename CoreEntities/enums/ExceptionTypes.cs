using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.enums
{
   
        public enum ExceptionTypeEnum
        {
            Exception = 0,
            LogoutWithAjaxRequest = 1
        }

        public enum UserLoginMessage
        {
            NotExits = 0,
            InActive = 1,
            IsDeleted = 2,
            ValidUser = 3
        }
        public enum ExpeptionTypes
        {
            LoginException = 0,
            CustomMessage = 1,
            CustomException = 2
        }
}
