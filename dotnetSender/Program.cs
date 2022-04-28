using System;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace dotnetSender
{
    class Program
    {
         static void Main(string[] args)
         {

            var rand = new Random();

            DateTime RandomDay()
            {
                DateTime start = new DateTime(2022, 4, 28);
                DateTime end = new DateTime(2024, 4, 28);
                int range = (end - start).Days;
                return start.AddDays(rand.Next(range));
            }

            var day = RandomDay();

            var reservation = new Reservation();

            if (args.Length == 3) { 

                reservation = new Reservation() {
                    hotelId = 1,
                    checkIn = day.ToString("O"),
                    checkOut = day.AddDays(rand.Next(1, 7)).ToString("O"),
                    customerName = args[0],
                    customerEmail = args[1],
                    customerAddress = args[2],
                    roomNo = rand.Next(100, 999)
                };

            } 
            else
            {
                reservation = new Reservation()
                {
                    hotelId = 1,
                    checkIn = day.ToString("O"),
                    checkOut = day.AddDays(rand.Next(1, 7)).ToString("O"),
                    customerName = "Default",
                    customerEmail = "Default",
                    customerAddress = "Default",
                    roomNo = rand.Next(100, 999)
                };
            }

            var factory = new ConnectionFactory() { HostName = "amqps://xdcsxmjy:WEhsUfZPjDfyw8qatdH81MCfa8wB0xfK@hawk.rmq.cloudamqp.com/xdcsxmjy" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "reservations",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    var message = JsonSerializer.Serialize(reservation);
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                 routingKey: "hello",
                                 basicProperties: null,
                                 body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
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
