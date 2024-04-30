using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using System.Text;
using ResourceWatcher.Hubs;
using RabbitMQ.Client.Events;

namespace ResourceWatcher.Services
{
    public class RabbitMQMessageService : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly IMessageService _messageService;

        public RabbitMQMessageService(IHubContext<MessageHub> hubContext)
        {
            _hubContext = hubContext;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "fileChanges",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("MQ Service started");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
                await _messageService.SaveMessageAsync(message);
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            _channel.BasicConsume(queue: "fileChanges", autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }

        public string ReceiveMessage()
        {
            var result = _channel.BasicGet("fileChanges", true);
            if (result != null)
            {
                return Encoding.UTF8.GetString(result.Body.ToArray());
            }
            return null;
        }
    }
}