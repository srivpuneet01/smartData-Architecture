using ServiceLayer.Interfaces;
using System;
using System.Net.Mail;
using System.Threading;

namespace ServiceLayer.Email
{
    public class EmailMessageSender : IMessageSender
    {
        private readonly IConfig _config;
        private SmtpClient smtpClient;
        public EmailMessageSender(IConfig config)
        {
            _config = config;
            GetSmtpCredentails();
        }

        /// <summary>
        /// get Exoandly smtp credentails from config file
        /// </summary>
        public void GetSmtpCredentails()
        {
            smtpClient = new SmtpClient();
            smtpClient.Host = _config.MailServer;
            smtpClient.Port = Convert.ToInt32(_config.MailServerPort);
            smtpClient.Credentials = new System.Net.NetworkCredential(_config.FromAddress, _config.FromAddresspassword);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = Convert.ToBoolean(_config.EnableSsl);
        }

        /// <summary>
        /// method for sending emails without cc or bcc
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        public void Send(string fromAddress, string toAddress, string subject, string message)
        {
            Send(fromAddress, toAddress, subject, message, string.Empty, string.Empty);
        }

        /// <summary>
        /// method for sending emails with cc or bcc
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="carbonCopyAddress"></param>
        /// <param name="blindCarbonCopyAddress"></param>
        public void Send(string fromAddress, string toAddress, string subject, string message, string carbonCopyAddress, string blindCarbonCopyAddress)
        {
            var emailMessage = BuildEmailMessage(fromAddress, toAddress, subject, message, carbonCopyAddress, blindCarbonCopyAddress);
            smtpClient.Send(emailMessage);
        }

        /// <summary>
        /// method for sending emails asynchronously without cc or bcc
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        public void SendAsync(string fromAddress, string toAddress, string subject, string message)
        {
            SendAsync(fromAddress, toAddress, subject, message, string.Empty, string.Empty);
        }

        /// <summary>
        /// method for sending emails asynchronously with cc or bcc
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="carbonCopyAddress"></param>
        /// <param name="blindCarbonCopyAddress"></param>
        public void SendAsync(string fromAddress, string toAddress, string subject, string message, string carbonCopyAddress, string blindCarbonCopyAddress)
        {
            var emailMessage = BuildEmailMessage(fromAddress, toAddress, subject, message, carbonCopyAddress, blindCarbonCopyAddress);

            SendAsyncEmail(emailMessage);
        }

        /// <summary>
        /// core method to build email message
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="carbonCopyAddress"></param>
        /// <param name="blindCarbonCopyAddress"></param>
        /// <returns></returns>
        private static MailMessage BuildEmailMessage(string fromAddress, string toAddress, string subject, string message,
                                                     string carbonCopyAddress, string blindCarbonCopyAddress)
        {
            var emailMessage = new MailMessage(fromAddress, toAddress);
            if (!string.IsNullOrWhiteSpace(carbonCopyAddress))
                emailMessage.CC.Add(carbonCopyAddress);
            if (!string.IsNullOrWhiteSpace(blindCarbonCopyAddress))
                emailMessage.Bcc.Add(blindCarbonCopyAddress);

            emailMessage.Subject = subject;
            emailMessage.Body = message;
            emailMessage.IsBodyHtml = true;
            return emailMessage;
        }

        private class AsyncArgs
        {
            public MailMessage MailMessage { get; set; }
            public SmtpClient SmtpClient { get; set; }
        }

        /// <summary>
        /// Sends an email asynchronously using the System.Threading.Threadpool mechanism.
        /// The thread that executes the email send is a child of the w3wp process,
        ///     so if the process recycles / dies in the middle of a thread's execution, 
        ///     the thread will abort suddenly (possibly without sending the email).
        /// Since email sends only take a few seconds, this low risk is acceptable.
        ///     the alternative would be to write our own thread pool, which will likely be very time consuming.
        /// </summary>
        private void SendAsyncEmail(MailMessage mailMessage)
        {
            var asyncArgs = new AsyncArgs()
            {
                SmtpClient = smtpClient,
                MailMessage = mailMessage
            };
            ThreadPool.QueueUserWorkItem(SendAsyncEmailCallBack, asyncArgs);
        }

        private static void SendAsyncEmailCallBack(object state)
        {

            /*Use can use elmah to log the response or any custom logger tool or you can just write it to simple text file
            Below is an example to log respons eusing NLog. */


            //var asyncArgs = (AsyncArgs)state;
            //don't rely on the web request's DI'd logger, get our own logger explicitly here
            //ILog log =
            //    ((ILoggerFactoryAdapter)
            //        GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILoggerFactoryAdapter)))
            //        .GetLogger(typeof(EmailMessageSender));

            //try
            //{
            //    asyncArgs.SmtpClient.Send(asyncArgs.MailMessage);
            //    log.InfoFormat("Email successfully send. To: {0}. Subject: {1}", asyncArgs.MailMessage.To,
            //        asyncArgs.MailMessage.Subject);
            //}
            //catch (Exception ex)
            //{
            //    log.ErrorFormat("Error sending email from async thread. Message: {0}", ex, ex.Message);
            //    //swallow exception because we're in a threadpool thread and there's nothing to catch it anyway
            //    //also, I throwing an exception here can crash the w3wp process.
            //}
        }

    }
}