﻿using System;
using RabbitMQ.Client;
using System.Text;

namespace Send
{
    class Send
    {
        /*
        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }
        */
        static void Main(string[] args)
        {
            //создание соединения к серверу
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                //using (var channel = factory.CreateConnection().CreateModel())
                using (var channel = connection.CreateModel())
                {
                    //объявление обмена logs
                    channel.ExchangeDeclare("direct_logs", "direct");

                    // объявили очередь channel.QueueDeclare(string queue, bool durable, bool exclusive, bool autoDeleted, arguments);
                    //durable - надежность ( при true если rabbit обвалится, то очередь сохранится)
                    //channel.QueueDeclare("task_queue1", true, false, false, null);

                    

                    string message = "";

                    
                    do
                    {
                        
                        Console.WriteLine("Enter RoutingKey: info, eerror, warning");
                        var sc = Console.ReadLine();
                        if ((sc!="info") && (sc!="error") && (sc != "warning"))
                        {
                            Console.WriteLine("Usage: info, eerror, warning");
                            continue;
                        }
                        var severity = sc;

                        Console.Write("Enter the message:");
                        message = Console.ReadLine();
                        //message = GetMessage(args);
                        
                        //перевод message в массив байтов
                        var body = Encoding.UTF8.GetBytes(message);
                                                
                        //публикует сообщение
                        channel.BasicPublish("direct_logs", severity, null, body);

                        Console.WriteLine("[x] sent {0} {1}", severity, message);
                    } while (message != "end");
                }

                Console.WriteLine("press 'enter' to exit ");
                Console.ReadLine();

            }

           
        }
    }
}
