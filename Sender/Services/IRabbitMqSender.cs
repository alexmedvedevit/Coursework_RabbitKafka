namespace Sender.Services
{
    public interface IRabbitMqSender
    {
        void MessageSend<T>(T message);
    }

}
