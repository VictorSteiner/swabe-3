using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetHandler
{
    public class HotelContext : DbContext
    {
        public DbSet<Reservation> Reservations { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ABEHotel;Integrated Security=True;");
    }
    
    [Keyless]
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
