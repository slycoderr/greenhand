using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security;
using System.Web;

namespace GreenHand.Utility
{
    internal class SecurityHelpers
    {
        internal static string ValidateToken(string tokenString)
        {
            var token = new JwtSecurityToken(tokenString);
            var username = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (DateTime.Now > token.ValidTo)
            {
                throw new SecurityException("Token is invalid");
            }

            return username;
        }
    }
}