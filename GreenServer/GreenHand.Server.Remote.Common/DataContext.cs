using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenHand.Portable.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenHand.Server.Remote.Common
{
    public class GreenHandContext : DbContext
    {
        public DbSet<SensorValue> SensorValues { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=data.db");
        }
    }
}
