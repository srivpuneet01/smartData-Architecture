using ServiceLayer.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.Services
{
    /// <summary>
    /// notes about utcnow vs now
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EmailService : IEmailService
    {
        private readonly IConfig _config;
        private readonly IMessageFormatter _messageFormatter;
        private readonly IMessageSender _messageSender;


        public EmailService(IConfig config, IMessageFormatter messageFormatter, IMessageSender messageSender)
        {
            _config = config;
            _messageFormatter = messageFormatter;
            _messageSender = messageSender;
        }

        /// <summary>
        /// method to configure the html and email for registration email
        /// </summary>
        /// <param name="fullname"></param>
        /// <param name="storename"></param>
        /// <param name="email"></param>
        /// <param name="toAddress"></param>
        public void SendRegistrationEmail(string fullname, string email, string toAddress)
        {
            var mailMessage = _messageFormatter.BuildRegistrationMessage(fullname, email);
            _messageSender.SendAsync(_config.FromAddress, toAddress, "Welcome to Smartdata", mailMessage, _config.CC_RegistrationEmail, _config.BCC_RegistrationEmail);
        }

    }
}
