using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Templates;
using System.Collections.Generic;

namespace ServiceLayer.Email
{
    public class HtmlMessageFormatter : IMessageFormatter
    {
        private readonly IConfig _config;

        public HtmlMessageFormatter(IConfig config)
        {
            _config = config;
        }

        

        /// <summary>
        /// build html for registration email
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="storeName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public string BuildRegistrationMessage(string fullName, string email)
        {
            var template = new Registration();
            template.Session = new Dictionary<string, object>();
            template.Session.Add("HostDomain", _config.SiteUrl);
            template.Session.Add("FullName", fullName);
            template.Session.Add("Email", email);
            template.Initialize();
            return template.TransformText();
        }

    }
}