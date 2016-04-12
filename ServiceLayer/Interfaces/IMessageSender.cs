namespace ServiceLayer.Interfaces
{
    public interface IMessageSender
    {
        void Send(string fromAddress, string toAddress, string subject, string message);
        void Send(string fromAddress, string toAddress, string subject, string message, string carbonCopyAddress, string blindCarbonCopyAddress);
        void SendAsync(string fromAddress, string toAddress, string subject, string message);
        void SendAsync(string fromAddress, string toAddress, string subject, string message, string carbonCopyAddress, string blindCarbonCopyAddress);
    }
}
