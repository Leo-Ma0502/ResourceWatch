using RabbitMQ.Client;
using System.Text;

namespace ResourceWatcher.Services
{
    public class RabbitMQMessageService
    {
        private readonly IConnection connection;
        private readonly IModel channel;

        public RabbitMQMessageService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(queue: "fileChanges",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "",
                                 routingKey: "hello",
                                 basicProperties: null,
                                 body: body);
        }

        public string ReceiveMessage()
        {
            var result = channel.BasicGet("fileChanges", true);
            if (result != null)
            {
                return Encoding.UTF8.GetString(result.Body.ToArray());
            }
            return null;
        }


        public void Dispose()
        {
            channel?.Close();
            connection?.Close();
        }
    }
}