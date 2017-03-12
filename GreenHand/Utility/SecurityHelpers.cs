using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http.Headers;
using System.Security;
using System.Web;

namespace GreenHand.Utility
{
    internal class SecurityHelpers
    {
        internal static string ValidateToken(string tokenString)
        {
            tokenString = tokenString.Replace("bearer ", "");

            var token = new JwtSecurityToken(tokenString);
            var username = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (DateTime.Now > token.ValidTo)
            {
                throw new SecurityException("Token is invalid");
            }

            return username;
        }

        internal static int ValidateToken(HttpRequestHeaders headers)
        {
            //if (!headers.Contains("Authorization") || headers.FirstOrDefault(h => h.Key == "Authorization").Value?.FirstOrDefault() == null)
            //{
            //    throw new SecurityException("Token is invalid");
            //}

            string headerToken = headers.FirstOrDefault(h => h.Key == "Authorization").Value?.FirstOrDefault();

            if (string.IsNullOrEmpty(headerToken))
            {
                throw new SecurityException("Token is invalid");
            }

            headerToken = headerToken.Replace("bearer ", "");

            var token = new JwtSecurityToken(headerToken);
            var username = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            int id;

            if (string.IsNullOrEmpty(username))
            {
                throw new SecurityException("Token is invalid");
            }

            if (!int.TryParse(username, out id))
            {
                throw new SecurityException("Token is invalid");
            }

            if (DateTime.Now > token.ValidTo)
            {
                throw new SecurityException("Token is invalid");
            }

            return id;
        }
    }
}