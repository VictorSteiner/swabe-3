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

        public string DbPath { get; }

        public HotelContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "hotel.db");
            Console.WriteLine(DbPath);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
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
