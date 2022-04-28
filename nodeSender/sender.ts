import { connect } from "amqplib/callback_api";
import { config } from "dotenv";
import { exit } from "process";
import { generateReservation } from "./generate";

config();

connect(process.env.AMQP_CLOUD, (err, connection) => {
  if (err) throw err;

  connection.createChannel((err, channel) => {
    if (err) throw err;

    channel.assertQueue("reservations", {}, (err, ok) => {
      if (err) throw err;

      const args = process.argv.slice(2, 4);

      const reservation = generateReservation(
        args.length === 3
          ? { name: args[0], email: args[1], address: args[2] }
          : undefined
      );

      const correlationId = Math.random().toString();

      console.log("Sending message");

      channel.consume(
        ok.queue,
        (msg) => {
          if (msg.properties.correlationId == correlationId) {
            console.log(msg.content.toString());
            setTimeout(() => {
              connection.close();
              process.exit(0);
            }, 500);
          }
        },
        {
          noAck: true,
        },
        console.log
      );

      channel.sendToQueue(
        "reservations",
        Buffer.from(JSON.stringify(reservation)),
        {
          correlationId,
          replyTo: ok.queue,
        }
      );
    });
  });
});
