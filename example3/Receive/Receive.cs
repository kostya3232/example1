using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;

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
                    channel.ExchangeDeclare("logs", "fanout");

                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queueName, "logs", "");

                    Console.WriteLine("waiting for messeges");
                    
                    //отправка сообщения незанятому работнику
                    //channel.BasicQos(0, 1, false);

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

                        //считаеем количество точек n в сообщении и "усыпляем" процесс на 
                        //n секунд (симуляция времени выполнения запроса на сервере)
                        //int dots = message.Split('.').Length - 1;
                        //Thread.Sleep(dots * 1000);

                        //Console.WriteLine("[x] Done");

                        //подтверждение получения и выполнения сообщения
                        //channel.BasicAck(ea.DeliveryTag, false);

                    };

                    //true - без сохранения сообщений(если работник уйдет, то сообщения потеряются)
                    //false - с сохранением (если работник уйдет все его сообщения будут отправлены другим работникам)
                    channel.BasicConsume(queueName, true, consumer);

                    Console.WriteLine("press 'Enter' to exit");
                    Console.ReadLine();

                }
            }
        }
    }
}