using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using GreenHand.Portable;
using GreenHand.Portable.Models;
using GreenHand.Server.Remote.Common.SensorApi;
using GreenHand.Server.Remote.Common.UserApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreenHand.Tests
{
    [TestClass]
    public class ServerTests
    {
        [TestMethod]
        public async Task InsertSensorValue()
        {
            SensorApi api = new SensorApi();

            await api.StoreSensorData(new SensorValue(){Timestamp = DateTime.Now, ReadingType = SensorReadingType.Humidity, ReadResult = 50});
        }

        [TestMethod]
        public async Task GetAllSensorValues()
        {
            SensorApi api = new SensorApi();

            var results = (await api.GetSensorValues()).ToList();

            Assert.IsTrue(results.Any(), "Count:" +results.Count);
        }

        [TestMethod]
        public async Task RegisterUser()
        {
            UserApi api = new UserApi();

            var results = await api.CreateUser("slycoder127@hotmail.com", "test123");

            Assert.IsNotNull(results, "user was null");
        }

        [TestMethod]
        public async Task RegisterUser2()
        {
            string sqlCmdText = @"INSERT INTO [dbo].[Users] ([email], [password]) VALUES (@email, @password);";

            SqlCommand sqlCmd = new SqlCommand(sqlCmdText);


            SqlParameter paramSSN = new SqlParameter(@"@email", "slycoder127@hotmail.com");
            paramSSN.DbType = DbType.AnsiString;
            paramSSN.Direction = ParameterDirection.Input;

            SqlParameter paramFirstName = new SqlParameter(@"@password", "test123");
            paramFirstName.DbType = DbType.AnsiString;
            paramFirstName.Direction = ParameterDirection.Input;

            sqlCmd.Parameters.Add(paramSSN);
            sqlCmd.Parameters.Add(paramFirstName);

            using (sqlCmd.Connection = new SqlConnection(@"Server=tcp:greenhand.database.windows.net,1433;Initial Catalog=GreenHand;Persist Security Info=False;User ID=akerti127;Password=M@gic345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Column Encryption Setting=Enabled;"))
            {

                    sqlCmd.Connection.Open();
                    sqlCmd.ExecuteNonQuery();

            }
        }
    }
}
