using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IEmailService
    {
        //void SendPasswordResetEmail(string token, string toAddress);
        void SendRegistrationEmail(string fullname, string email, string toAddress);
    }
}
