using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace dotnetReciever
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var reservation = JsonSerializer.Deserialize<Reservation>(message);
                        Console.WriteLine($"Dear {reservation.customerName} your reservation for {reservation.roomNo} has been confirmed");
                        Console.WriteLine($"You check in at {reservation.checkIn} and check out at {reservation.checkOut}, hope you enjoy your stay at {reservation.hotelId}");
                        Console.WriteLine($"You can see more details about your reservation, in your email spamfolder {reservation.customerEmail}");
                    };
                    channel.BasicConsume(queue: "hello",
                        autoAck: true,
                        consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
    public class Reservation
    {
        public int hotelId;
        public string checkIn;
        public string checkOut;
        public int roomNo;
        public string customerName;
        public string customerEmail;
        public string customerAddress;
    }
}
