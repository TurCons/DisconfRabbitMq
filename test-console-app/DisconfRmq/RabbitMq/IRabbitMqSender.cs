namespace DisconfRmq.RabbitMq
{
	public interface IRabbitMqSender
	{
		void SendMessage(object obj, string exchangeName, string routingKey, bool newConnection);

		void SendMessageWithNewConnection(string message, string exchangeName, string routingKey);

		void SendMessageWithoutNewConnection(string message, string exchangeName, string routingKey);
	}
}
