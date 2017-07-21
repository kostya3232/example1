using System;
using RabbitMQ.Client;
using System.Text;

namespace Send
{
    class Send
    {
        static void Main(string[] args)
        {
            //создание соединения к серверу
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                //using (var channel = factory.CreateConnection().CreateModel())
                using (var channel = connection.CreateModel())
                {
                    // объявили очередь channel.QueueDeclare(string queue, bool durable, bool exclusive, bool autoDeleted, arguments);
                    //channel.QueueDeclare("queue", false, false, false, null);

                    channel.ExchangeDeclare("topic_logs", "topic");

                    string message = "";

                    do
                    {
                        Console.WriteLine("Enter topic");
                        var tp = Console.ReadLine();
                        var routingKey = tp;
                        Console.WriteLine("Enter message");
                        message = Console.ReadLine();
                        //перевод message в массив байтов
                        var body = Encoding.UTF8.GetBytes(message);

                        //публикует сообщение
                        channel.BasicPublish("topic_logs", routingKey, null, body);

                        Console.WriteLine("[x] sent {0} {1}",routingKey, message);
                    } while (message != "end");

                }

                Console.WriteLine("press 'enter' to exit ");
                Console.ReadLine();

            }
        }
    }
}
