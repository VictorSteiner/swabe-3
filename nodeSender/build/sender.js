"use strict";
exports.__esModule = true;
var callback_api_1 = require("amqplib/callback_api");
var dotenv_1 = require("dotenv");
var generate_1 = require("./generate");
(0, dotenv_1.config)();
(0, callback_api_1.connect)(process.env.AMQP_CLOUD, function (err, connection) {
    if (err)
        throw err;
    connection.createChannel(function (err, channel) {
        if (err)
            throw err;
        channel.assertQueue("reservations", {}, function (err, ok) {
            if (err)
                throw err;
            var args = process.argv.slice(2, 4);
            var reservation = (0, generate_1.generateReservation)(args.length === 3
                ? { name: args[0], email: args[1], address: args[2] }
                : undefined);
            var correlationId = Math.random().toString();
            console.log("Sending message");
            channel.consume(ok.queue, function (msg) {
                if (msg.properties.correlationId == correlationId) {
                    console.log(msg.content.toString());
                    setTimeout(function () {
                        connection.close();
                        process.exit(0);
                    }, 500);
                }
            }, {
                noAck: true
            }, console.log);
            channel.sendToQueue("reservations", Buffer.from(JSON.stringify(reservation)), {
                correlationId: correlationId,
                replyTo: ok.queue
            });
        });
    });
});
