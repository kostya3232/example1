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
                    channel.QueueDeclare("queue", false, false, false, null);

                    string message = "Hello World";
                    //перевод message в массив байтов
                    var body = Encoding.UTF8.GetBytes(message);

                    //публикует сообщение
                    channel.BasicPublish("", "queue", null, body);

                    Console.WriteLine("[x] sent {0}", message);

                }

                Console.WriteLine("press 'enter' to exit ");
                Console.ReadLine();

            }
        }
    }
}
