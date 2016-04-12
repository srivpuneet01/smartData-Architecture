using ServiceLayer.Interfaces;
using System.Configuration;

namespace ServiceLayer.Email
{
    public class Config : IConfig
    {
        public Config()
        {
            MailServer = ConfigurationManager.AppSettings["MailServer"];
            MailServerPort = ConfigurationManager.AppSettings["MailServerPort"];
            FromAddress = ConfigurationManager.AppSettings["FromAddress"];
            FromAddresspassword = ConfigurationManager.AppSettings["FromAddresspassword"];
            EnableSsl = ConfigurationManager.AppSettings["EnableSsl"];
            CC_RegistrationEmail = ConfigurationManager.AppSettings["CC_RegistrationEmail"];
            BCC_RegistrationEmail = ConfigurationManager.AppSettings["BCC_RegistrationEmail"];
            SiteUrl = ConfigurationManager.AppSettings["SiteUrl"];
        }
        public string MailServer { get; protected set; }
        public string MailServerPort { get; protected set; }
        public string FromAddress { get; protected set; }
        public string FromAddresspassword { get; protected set; }
        public string EnableSsl { get; protected set; }
        public string CC_RegistrationEmail { get; protected set; }
        public string BCC_RegistrationEmail { get; protected set; }
        public string SiteUrl { get; protected set; }
    }
}