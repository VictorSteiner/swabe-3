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
      var factory = new ConnectionFactory() { HostName = "hawk.rmq.cloudamqp.com", UserName = "xdcsxmjy", VirtualHost = "xdcsxmjy", Password = "WEhsUfZPjDfyw8qatdH81MCfa8wB0xfK" };
      using (var connection = factory.CreateConnection())
      {
        using (var channel = connection.CreateModel())
        {
          channel.QueueDeclare(queue: "confirmations",
              durable: true,
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
            Console.WriteLine($"You can see more details about your reservation, in your email spamfolder, {reservation.customerEmail}");
          };
          channel.BasicConsume(queue: "confirmations",
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
    public int hotelId { get; set; }
    public string checkIn { get; set; }
    public string checkOut { get; set; }
    public int roomNo { get; set; }
    public string customerName { get; set; }
    public string customerEmail { get; set; }
    public string customerAddress { get; set; }
  }
}
