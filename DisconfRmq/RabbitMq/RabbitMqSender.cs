namespace DisconfRmq.RabbitMq
{
	using RabbitMQ.Client;
    using System;
    using System.Collections.Generic;
    using System.Text;
	using System.Text.Json;

	public class RabbitMqSender : IRabbitMqSender
	{
		public void SendMessage(object obj)
		{
			var message = JsonSerializer.Serialize(obj);
			SendMessage(message);
		}

		public void SendMessage(string message)
		{
			// Не забудьте вынести значения "localhost" и "MyQueue"
			// в файл конфигурации
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				string queueName = "q1"; // "queue-" + Guid.NewGuid().ToString();
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

				channel.BasicPublish(exchange: "ex2",
							   routingKey: "",
							   basicProperties: basicProperties,
							   body: body);
			}
		}
	}
}
