namespace DisconfRmq.RabbitMq
{
	public interface IRabbitMqSender
	{
		void SendMessage(object obj, string exchangeName, string routingKey);

		void SendMessage(string message, string exchangeName, string routingKey);
	}
}
