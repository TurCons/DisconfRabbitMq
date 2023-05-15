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
            Console.WriteLine("Sending messages to RMQ!");

            for (int i = 0; i < 10; i++)
            {
                rmqSender.SendMessage($"Message {i} !!!", "exchange1", "routingKey1");
            }
            Console.WriteLine("Sended messages to exchange1.routingKey1.");

            for (int i = 0; i < 10; i++)
            {
                var testObject = new TestDtoClass();
                rmqSender.SendMessage(testObject, "exchange2", "routingKey2");
            }
            Console.WriteLine("Sended messages to exchange1.routingKey2.");
            
            // прием пачки сообщений из кролика
            RabbitMqListener rabbitMqListener = new RabbitMqListener(host, port, userName, password, "queue1");
            var cancellationToken = new CancellationToken();
            rabbitMqListener.StartAsync(cancellationToken);

            // Console.ReadKey(true);
            Console.Read();
        }
    }
}
