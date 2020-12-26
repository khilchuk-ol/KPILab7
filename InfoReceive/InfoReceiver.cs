using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using MongoDB.Driver;

namespace InfoReceive
{
    public class InfoReceiver
    {
        static void Main()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("ErrorRabbitDB");
            var collection = db.GetCollection<Message>("messages");

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
                                      routingKey: "info");

                    Console.WriteLine(" [*] Waiting for messages.");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, eargs) =>
                    {
                        var body = eargs.Body.ToArray();
                        var json = Encoding.UTF8.GetString(body);
                        var msg = JsonConvert.DeserializeObject<Message>(json);

                        collection.InsertOne(msg);

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
