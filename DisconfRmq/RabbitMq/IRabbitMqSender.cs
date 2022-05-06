namespace DisconfRmq.RabbitMq
{
	public interface IRabbitMqSender
	{
		void SendMessage(object obj);
		void SendMessage(string message);
	}
}
