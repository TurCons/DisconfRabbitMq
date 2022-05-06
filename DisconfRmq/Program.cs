namespace DisconfRmq
{
    using System;
    using System.Threading;
    using DisconfRmq.RabbitMq;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sending 100 messages to RMQ!");

            var rmqSender = new RabbitMqSender();
            for (int i = 0; i < 100; i++)
            {
                rmqSender.SendMessage($"Message {i} !!!");
            }

            Console.WriteLine("Sended messages to RMQ!");
            /*
            RabbitMqListener rabbitMqListener = new RabbitMqListener();

            var cancellationToken = new CancellationToken();

            rabbitMqListener.StartAsync(cancellationToken);
            */
            Console.ReadKey(true);
        }
    }
}
