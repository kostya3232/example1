using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Receive
{
    class Receive
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("queue", false, false, false, null);

                    //обработчик событий??
                    var consumer = new EventingBasicConsumer(channel);

                    //при получении сообщения подписываемся на событие (+=), 
                    //т.е. в данном случае выполняем лямбда выражение (=>), 
                    //в котором входные параметры model и ea и в {} тело программы
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine("[x] Reseived {0}", message);
                    };

                    channel.BasicConsume("queue", true, consumer);

                    Console.WriteLine("press 'Enter' to exit");
                    Console.ReadLine();
                                    
                }
            }
        }
    }
}
