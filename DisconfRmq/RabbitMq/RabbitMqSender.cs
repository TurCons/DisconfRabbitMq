namespace DisconfRmq.RabbitMq
{
	using RabbitMQ.Client;
    using System;
    using System.Collections.Generic;
    using System.Text;
	using System.Text.Json;

	public class RabbitMqSender : IRabbitMqSender
	{
		private ConnectionFactory _factory;

		public RabbitMqSender(string host, int port, string username, string password)
		{
			_factory  = new ConnectionFactory { HostName = host, Port = port, UserName = username, Password = password };
		}

		public void SendMessage(object obj, string exchangeName, string routingKey)
		{
			var message = JsonSerializer.Serialize(obj);
			SendMessage(message, exchangeName, routingKey);
		}

		public void SendMessage(string message, string exchangeName, string routingKey)
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

				channel.BasicPublish(exchange: exchangeName,
							   routingKey: routingKey,
							   basicProperties: basicProperties,
							   body: body);
			}
		}
	}
}
