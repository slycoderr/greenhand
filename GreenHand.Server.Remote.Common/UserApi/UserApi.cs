using System;
using System.Data.Entity;
using System.Threading.Tasks;
using GreenHand.Portable.Models;
using GreenHand.Server.Remote.Common.Security;

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
                if (await db.Users.FirstOrDefaultAsync(u => u.Email == email) != null)
                {
                    throw new ArgumentException("That email address is in use.");
                }

                var salt = Guid.NewGuid().ToString();
                var protectedPassword = CryptoHelper.EncryptString(password, salt);

                var user = new User {Email = email, Password = protectedPassword, Salt = salt, ApiKey = (Guid.NewGuid().ToString()+ Guid.NewGuid().ToString()+ Guid.NewGuid().ToString()) };

                db.Users.Add(user);

                await db.SaveChangesAsync();
            }
        }
    }
}