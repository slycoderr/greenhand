using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using GreenHand.Portable.Models;

namespace GreenHand.Server.Remote.Common
{
    public class GreenHandContext : DbContext
    {
        public DbSet<SensorValue> SensorValues { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<User> Users { get; set; }

        //ef7
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    optionsBuilder.UseSqlServer(@"Server=tcp:greenhand.database.windows.net,1433;Initial Catalog=GreenHand;Persist Security Info=False;User ID=akerti127;Password=M@gic345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        //    //await Database.EnsureCreatedAsync();
        //}

        public GreenHandContext(): base(@"Server=tcp:greenhand.database.windows.net,1433;Initial Catalog=GreenHand;Persist Security Info=False;User ID=akerti127;Password=M@gic345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Column Encryption Setting=Enabled;")
        {

            //Dictionary<string, SqlColumnEncryptionKeyStoreProvider> providers =
            //   new Dictionary<string, SqlColumnEncryptionKeyStoreProvider>();
            //providers.Add("MY_CUSTOM_STORE", customProvider);
            //SqlConnection.RegisterColumnEncryptionKeyStoreProviders(providers);
            //providers.Add(SqlColumnEncryptionCertificateStoreProvider.ProviderName, customProvider);
            //SqlConnection.RegisterColumnEncryptionKeyStoreProviders(providers);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>().Ignore(u=>u.)
            modelBuilder.Entity<User>().Property(x => x.Email).HasColumnType("varchar(MAX)");
            modelBuilder.Entity<User>().Property(x => x.Password).HasColumnType("varchar(MAX)");
        }
    }
}