namespace DisconfRmq.RabbitMq
{
	using RabbitMQ.Client;
    using System;
    using System.Collections.Generic;
    using System.Text;
	using System.Text.Json;

	public class RabbitMqSender : IRabbitMqSender, IDisposable
	{
		private ConnectionFactory _factory;

		private IConnection _connection;

		private IModel _channel;


		public RabbitMqSender(string host, int port, string username, string password)
		{
			_factory  = new ConnectionFactory { HostName = host, Port = port, UserName = username, Password = password };

			_connection = _factory.CreateConnection();
			_channel = _connection.CreateModel();
		}

        public void Dispose()
        {
			_connection.Close();
			_channel.Close();
        }

        public void SendMessage(object obj, string exchangeName, string routingKey)
		{
			var message = JsonSerializer.Serialize(obj);
			SendMessage(message, exchangeName, routingKey);
		}

		public void SendMessage(string message, string exchangeName, string routingKey)
		{
			// string queueName = "q1"; // "queue-" + Guid.NewGuid().ToString();
			// _channel.QueueDeclare(queue: queueName,
			// 				durable: false,
			// 				exclusive: false,
			// 				autoDelete: false,
			// 				arguments: null);

			var body = Encoding.UTF8.GetBytes(message);

			var basicProperties = _channel.CreateBasicProperties();
			basicProperties.Headers = new Dictionary<string, object>();
			basicProperties.Headers.Add("qwe", "qwe123");
			basicProperties.Headers.Add("sendedType", "channelInConstructor");
			basicProperties.DeliveryMode = 2;

			_channel.BasicPublish(exchangeName, routingKey, basicProperties, body);
		}
	}
}
