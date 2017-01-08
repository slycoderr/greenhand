using GreenHand.Portable.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenHand.Server.Remote.Common
{
    public class GreenHandContext : DbContext
    {
        public DbSet<SensorValue> SensorValues { get; set; }

        protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=data.db");

            //await Database.EnsureCreatedAsync();
        }
    }
}