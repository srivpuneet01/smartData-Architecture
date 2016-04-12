using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class LoginException : Exception
    {
        private String message;
        public LoginException(String _message)
        {
            Message = _message;
        }
        public String Message
        {
            get { return message; }
            set { message = value; }

        }
    }
    public class CustomMessage : Exception
    {
        
        private String message;
        public CustomMessage(String _message)
            : base(_message)
        {
            Message = _message;
        }
        public String Message
        {
            get { return message; }
            set { message = value; }

        }
    }
    public class CustomException : Exception
    {
        private String message;
        //private Exception exception;
        public CustomException( String _message)
        {
            //Exception = _exception;
            Message = _message;
        }
        public String Message
        {
            get { return message; }
            set { message = value; }
        }
        //public Exception Exception
        //{
        //    get { return exception; }
        //    set { exception = value; }
        //}
    }
}
