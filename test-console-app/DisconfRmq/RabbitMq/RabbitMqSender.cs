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

		public void CreateExchange(string exchangeName, string bindingKey, string queueName) {
			var queueName_tmp = queueName;
			try 
			{
				// вернет наименование очереди или количество сообщений в ней или свалится в исключение, если очереди нет ,
				// аналогичные методы есть exchange

				var info = _channel.QueueDeclarePassive(queueName_tmp);
				Console.WriteLine($"Очередь {info.QueueName} найдена и в ней вот столько сообщений: {info.MessageCount}");

				queueName_tmp = "wrongQueue";
				var info1 = _channel.QueueDeclarePassive("");
				Console.WriteLine($"Очередь {info.QueueName} найдена и в ней вот столько сообщений: {info.MessageCount}");
			} 
			catch (Exception ex)
			{
				Console.WriteLine($"Очередь {queueName_tmp} не найдена");
				
				if (_channel.IsClosed)
				{
					_channel = _connection.CreateModel();
				}
			} 

			_channel.QueueDeclare(queue: queueName,
							durable: false,
							exclusive: false,
							autoDelete: false,
							arguments: null);

			_channel.ExchangeDeclare(exchange: exchangeName,
							type: "direct",
							durable: false,
							autoDelete: false
			);

			_channel.QueueBind(queue: queueName, 
							exchange: exchangeName,
							routingKey: bindingKey
			);
		}

        public void SendMessage(object obj, string exchangeName, string routingKey)
		{
			var message = JsonSerializer.Serialize(obj);
			SendMessage(message, exchangeName, routingKey);
		}

		public void SendMessage(string message, string exchangeName, string routingKey)
		{

			var body = Encoding.UTF8.GetBytes(message);

			var basicProperties = _channel.CreateBasicProperties();
			basicProperties.Headers = new Dictionary<string, object>();
			basicProperties.Headers.Add("qwe", "qwe123");
			basicProperties.Headers.Add("type", "test_message");

			basicProperties.DeliveryMode = 2;

			_channel.BasicPublish(exchangeName, routingKey, basicProperties, body);
		}
	}
}
