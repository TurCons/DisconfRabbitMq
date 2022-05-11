namespace DisconfRmq.RabbitMq
{
	using System;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	
	using RabbitMQ.Client.Events;
	using RabbitMQ.Client;
	using Microsoft.Extensions.Hosting;


	public class RabbitMqListener : BackgroundService
	{
		private IConnection _connection;
		private IModel _channel;
		private string _queueName;

		public RabbitMqListener(string host, int port, string username, string password, string queueName)
		{
			var factory = new ConnectionFactory { HostName = host, Port = port, UserName = username, Password = password };
			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();
			_queueName = queueName;
			//_channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += (ch, ea) =>
			{
				var content = Encoding.UTF8.GetString(ea.Body.ToArray());

				// Каким-то образом обрабатываем полученное сообщение
				Console.WriteLine($"Получено сообщение: {content}");

				_channel.BasicAck(ea.DeliveryTag, false);
			};

			bool autoAck = false;
			_channel.BasicConsume(_queueName, autoAck, consumer);

			return Task.CompletedTask;
		}

		public override void Dispose()
		{
			_channel.Close();
			_connection.Close();
			base.Dispose();
		}
	}
}
