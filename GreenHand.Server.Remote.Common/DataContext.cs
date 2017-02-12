using System;
using System.Data.Entity;
using GreenHand.Portable.Models;

namespace GreenHand.Server.Remote.Common
{
    public class GreenHandContext : DbContext
    {
        public DbSet<SensorValue> SensorValues { get; set; }

        //ef7
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    optionsBuilder.UseSqlServer(@"Server=tcp:greenhand.database.windows.net,1433;Initial Catalog=GreenHand;Persist Security Info=False;User ID=akerti127;Password=M@gic345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        //    //await Database.EnsureCreatedAsync();
        //}

        public GreenHandContext(): base(@"Server=tcp:greenhand.database.windows.net,1433;Initial Catalog=GreenHand;Persist Security Info=False;User ID=akerti127;Password=M@gic345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
        {
            
        }
    }
}