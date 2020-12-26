using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ErrorReceive
{
    public class ErrorReceiver
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

                    var queueName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queue: queueName,
                                      exchange: "direct_msg",
                                      routingKey: "error");

                    Console.WriteLine(" [*] Waiting for messages.");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, eargs) =>
                    {
                        var body = eargs.Body.ToArray();
                        var json = Encoding.UTF8.GetString(body);
                        var msg = JsonConvert.DeserializeObject<Message>(json);

                        using (var ctx = new AppContext())
                        {
                            ctx.Messages.Add(msg);
                            ctx.SaveChanges();
                        }

                        Console.WriteLine($" [x] Received '{eargs.RoutingKey}':'{msg.Text}' created at '{msg.CreateTime}' and received at '{msg.ReceiveTime}'");
                    };

                    channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}

