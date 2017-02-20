using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using GreenHand.Server.Remote.Common.UserApi;
using Microsoft.Owin.Security.OAuth;

namespace RestAPI
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.FromResult(context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            UserApi userApi = new UserApi();

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            try
            {
                var ok = await userApi.Login(context.UserName, context.Password);

                if (!ok)
                {
                    context.SetError("Login failure", "The user name or password is incorrect.");
                    context.Rejected();
                }

                else
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

                    context.Validated(identity);
                }
            }
            catch (Exception ex)
            {
                context.SetError("Login Exception", "An error occured while trying to login.");
                File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "log.txt"), ex.ToString());
                context.Rejected();
            }
        }
    }
}