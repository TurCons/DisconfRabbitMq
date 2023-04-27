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

        public void SendMessage(object obj, string exchangeName, string routingKey, bool newConnection)
		{
			var message = JsonSerializer.Serialize(obj);

			if (newConnection) {
				SendMessageWithNewConnection(message, exchangeName, routingKey);
			} else {
				SendMessageWithoutNewConnection(message, exchangeName, routingKey);
			} 
		}

		public void SendMessageWithNewConnection(string message, string exchangeName, string routingKey)
		{
			using (var connection = _factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				// string queueName = "q1"; // "queue-" + Guid.NewGuid().ToString();
				/*
				channel.QueueDeclare(queue: queueName,
							   durable: false,
							   exclusive: false,
							   autoDelete: false,
							   arguments: null);
				*/

				var body = Encoding.UTF8.GetBytes(message);

				var basicProperties = channel.CreateBasicProperties();
				basicProperties.Headers = new Dictionary<string, object>();
				basicProperties.Headers.Add("qwe", "qwe123");
				basicProperties.Headers.Add("sendedType", "newChannel");

				channel.BasicPublish(exchange: exchangeName,
							   routingKey: routingKey,
							   basicProperties: basicProperties,
							   body: body);
			}
		}

		public void SendMessageWithoutNewConnection(string message, string exchangeName, string routingKey)
		{
				var body = Encoding.UTF8.GetBytes(message);

				var basicProperties = _channel.CreateBasicProperties();
				basicProperties.Headers = new Dictionary<string, object>();
				basicProperties.Headers.Add("qwe", "qwe123");
				basicProperties.Headers.Add("sendedType", "channelInConstructor");
				basicProperties.DeliveryMode = 2;

				_channel.BasicPublish(exchange: exchangeName,
							   routingKey: routingKey,
							   basicProperties: basicProperties,
							   body: body);

		}
	}
}
