using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RPCServer
{
    class RPCServer
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //создаем очередь с отправлением сообщения незанятому работнику
                    channel.QueueDeclare("rpc_queue", false, false, false, null);
                    channel.BasicQos(0, 1, false);

                    //прием сообщения
                    var consumer = new EventingBasicConsumer(channel);
                    channel.BasicConsume("rpc_queue", false, consumer);
                    Console.WriteLine("waiting for RPC requests");

                    consumer.Received += (model, ea) =>
                    {
                        string response = "";

                        var body = ea.Body;
                        var props = ea.BasicProperties;                        
                        //id ответного сообщения
                        var replyProps = channel.CreateBasicProperties();
                        replyProps.CorrelationId = props.CorrelationId;

                        //считаем число фибоначи
                        try
                        {
                            var message = Encoding.UTF8.GetString(body);
                            int n = int.Parse(message);
                            Console.WriteLine("fib {0} ", message);
                            response = Fib(n).ToString();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("[.] " + e.Message);
                            response = "";
                        }
                        //отправляем результат клиенту
                        finally
                        {
                            var responseByte = Encoding.UTF8.GetBytes(response);
                            channel.BasicPublish("", props.ReplyTo, replyProps, responseByte);
                            channel.BasicAck(ea.DeliveryTag, false);
                        }
                    };
                    
                    Console.WriteLine("Press [enter] to exit");
                    Console.ReadLine();

                    

                }
            }
        }

        private static int Fib(int n)
        {
            if ((n == 0) || (n == 1)) return n;
            return Fib(n - 1) + Fib(n - 2);
        }
    }
}