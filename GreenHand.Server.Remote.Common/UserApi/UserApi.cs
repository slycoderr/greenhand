﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenHand.Portable.Models;

namespace GreenHand.Server.Remote.Common.UserApi
{
    public class UserApi
    {
        public async Task<bool> Login(string email, string password)
        {
            using (var db = new GreenHandContext())
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);

                return user != null && user.Password == password;
            }
        }

        public async Task<User> CreateUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email address cannot be empty.");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password address cannot be empty.");
            }

            using (var db = new GreenHandContext())
            {
                if (await db.Users.FirstOrDefaultAsync(u => u.Email == email) != null)
                {
                    throw new ArgumentException("That email address is in use.");
                }

                var user = new User {Email = email, Password = password};

                db.Users.Add(user);

                await db.SaveChangesAsync();

                user.Password = null;

                return user;
            }
        }
    }
}
