"use strict";
exports.__esModule = true;
exports.generateReservation = void 0;
var generateReservation = function (customer) {
    var checkIn = new Date(Date.UTC(2022, randomBetween(1, 6), randomBetween(1, 30))).toISOString();
    var checkOut = new Date(Date.UTC(2022, randomBetween(6, 12), randomBetween(1, 30))).toISOString();
    var reservation = {
        hotelId: Math.random(),
        checkIn: checkIn,
        checkOut: checkOut,
        roomNo: randomBetween(100, 999),
        customerName: customer ? customer.name : "default",
        customerEmail: customer ? customer.email : "default",
        customerAddress: customer ? customer.address : "default"
    };
    return reservation;
};
exports.generateReservation = generateReservation;
var randomBetween = function (min, max) {
    return Math.floor(Math.random() * (max - min + 1) + min);
};
