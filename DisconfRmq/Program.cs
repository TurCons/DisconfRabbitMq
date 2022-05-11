namespace DisconfRmq
{
    using System;
    using System.Threading;
    using DisconfRmq.RabbitMq;

    class Program
    {
        static void Main(string[] args)
        {
            var host = Environment.GetEnvironmentVariable("RmqHost");
            
            if (!int.TryParse(Environment.GetEnvironmentVariable("RmqPort"), out int port))
            {
                port = 5672;
            }
            
            var userName = Environment.GetEnvironmentVariable("RmqUserName");
            var password = Environment.GetEnvironmentVariable("RmqPassword");


            // отправка пачки сообщений в кролик.
            var rmqSender = new RabbitMqSender(host, port, userName, password);
            Console.WriteLine("Sending 10 messages to RMQ!");

            for (int i = 0; i < 10; i++)
            {
                rmqSender.SendMessage($"Message {i} !!!", "ex1", "");
            }

            Console.WriteLine("Sended messages to RMQ!");
            
            // прием пачки сообщений из кролика
            RabbitMqListener rabbitMqListener = new RabbitMqListener(host, port, userName, password, "q2");

            var cancellationToken = new CancellationToken();

            rabbitMqListener.StartAsync(cancellationToken);
            

            Console.ReadKey(true);
        }
    }
}
