using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using GreenHand.Portable.Models;
using GreenHand.Server.Remote.Common.Security;
using Environment = GreenHand.Portable.Models.Environment;

namespace GreenHand.Server.Remote.Common.UserApi
{
    public class UserApi
    {
        public async Task<bool> Login(User user)
        {
            using (var db = new GreenHandContext())
            {
                var dbUser = await db.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

                return dbUser != null && CryptoHelper.DecryptString(dbUser.Password, dbUser.Salt) == user.Password;
            }
        }

        public async Task CreateUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email address cannot be empty.");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be empty.");
            }

            if (password.Length < 8)
            {
                throw new ArgumentException("Password must be at least 8 characters long.");
            }

            using (var db = new GreenHandContext())
            {
                if (await db.Users.AnyAsync(u => u.Email == email))
                {
                    throw new ArgumentException("That email address is in use.");
                }

                var salt = Guid.NewGuid().ToString();
                var protectedPassword = CryptoHelper.EncryptString(password, salt);

                var user = new User {Email = email, Password = protectedPassword, Salt = salt, ApiKey = (Guid.NewGuid().ToString()+ Guid.NewGuid().ToString()+ Guid.NewGuid().ToString()) };

                db.Users.Add(user);

                var result = await db.SaveChangesAsync();

                if (result != 1)
                {
                    throw new Exception("Failed to insert user into database");
                }

                db.Environments.Add(new Environment { Name = "My Enivornment", UserId = user.Id });

                var result2 = await db.SaveChangesAsync();

                if (result2 != 1)
                {
                    throw new Exception("Failed to insert Environment into database");
                }
            }
        }
    }
}