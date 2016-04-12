using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IConfig
    {
        string MailServer { get; }
        string MailServerPort { get; }
        string FromAddress { get; }
        string FromAddresspassword { get; }
        string EnableSsl { get; }
        string CC_RegistrationEmail { get; }
        string BCC_RegistrationEmail { get; }
        string SiteUrl { get; }

    }
}
