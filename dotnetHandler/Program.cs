using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace dotnetHandler
{
  class Program
  {
    static void Main(string[] args)
    {
      var db = new HotelContext();
      var factory = new ConnectionFactory() { HostName = "hawk.rmq.cloudamqp.com", UserName = "xdcsxmjy", VirtualHost = "xdcsxmjy", Password = "WEhsUfZPjDfyw8qatdH81MCfa8wB0xfK" };

      using (var connection = factory.CreateConnection())
      {
        var channelConfirmations = connection.CreateModel();

        channelConfirmations.QueueDeclare(queue: "confirmations",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

        using (var channelReservations = connection.CreateModel())
        {
          channelReservations.QueueDeclare(queue: "reservations",
              durable: true,
              exclusive: false,
              autoDelete: false,
              arguments: null);

          var consumer = new EventingBasicConsumer(channelReservations);

          consumer.Received += (model, ea) =>
          {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var reservation = JsonSerializer.Deserialize<Reservation>(message);

            db.Add(reservation);
            db.SaveChanges();

            channelConfirmations.BasicPublish(exchange: "",
                                      routingKey: "confirmations",
                                      basicProperties: null,
                                      body: body);

            Console.WriteLine($"[x] Sent {message}");
          };

          channelReservations.BasicConsume(queue: "reservations",
              autoAck: true,
              consumer: consumer);


          Console.WriteLine(" Press [enter] to exit.");
          Console.ReadLine();
        };
      }
    }
  }
}
