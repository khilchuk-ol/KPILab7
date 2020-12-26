using System;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading;

namespace Send
{
    public class Sender
    {
        static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "direct_msg",
                                            type: "direct");

                    while(true)
                    {
                        Thread.Sleep(2000);

                        var msg = new Message();
                        var json = JsonConvert.SerializeObject(msg);
                        var body = Encoding.UTF8.GetBytes(json);

                        channel.BasicPublish(exchange: "direct_msg",
                                             routingKey: msg.ContentType.ToString(),
                                             basicProperties: null,
                                             body: body);

                        Console.WriteLine($" [x] Sent '{msg.ContentType}':'{msg.Text}' created at '{msg.CreateTime}'");
                    }
                }
            }            
        }
    }
}

