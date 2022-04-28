type Reservation = {
  hotelId: number;
  checkIn: string; // ISO 8601 date
  checkOut: string; // ISO 8601 date
  roomNo: number;
  customerName: string;
  customerEmail: string;
  customerAddress: string;
};

export type Customer = {
  name: string;
  email: string;
  address: string;
};

export const generateReservation = (customer?: Customer): Reservation => {
  const checkIn = new Date(
    Date.UTC(2022, randomBetween(1, 6), randomBetween(1, 30))
  ).toISOString();
  const checkOut = new Date(
    Date.UTC(2022, randomBetween(6, 12), randomBetween(1, 30))
  ).toISOString();

  const reservation: Reservation = {
    hotelId: Math.random(),
    checkIn,
    checkOut,
    roomNo: randomBetween(100, 999),
    customerName: customer ? customer.name : "default",
    customerEmail: customer ? customer.email : "default",
    customerAddress: customer ? customer.address : "default",
  };

  return reservation;
};

const randomBetween = (min: number, max: number) => {
  return Math.floor(Math.random() * (max - min + 1) + min);
};
