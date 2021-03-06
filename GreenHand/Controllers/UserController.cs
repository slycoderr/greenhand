﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using GreenHand.Portable.Models;
using GreenHand.Server.Remote.Common.UserApi;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;

namespace GreenHand.Controllers
{
    [RoutePrefix("user")]
    public class UserController : ApiController
    {
        [Route("register"), HttpPost]
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created, "The user was created.", typeof(User))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "An invalid parameter was entered.", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "An unkown error has occured.", Type = typeof(string))]
        public async Task<IHttpActionResult> PostUser([FromBody]User user)
        {
            UserApi api = new UserApi();

            await api.CreateUser(user.Email, user.Password);

            return Ok();

        }

        [Route("login"), HttpPost]
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created, "The user was authenticated.", typeof(string))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Invalid credentials supplied", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "An unkown error has occured.", Type = typeof(string))]
        public async Task<IHttpActionResult> LogintUser([FromBody]User user)
        {
            UserApi api = new UserApi();
            var userId = await api.Login(user);

            if (userId > 0) // user-defined function, checks against a database
            {

                    System.IdentityModel.Tokens.JwtSecurityToken token = Microsoft.Azure.Mobile.Server.Login.AppServiceLoginHandler.CreateToken(new Claim[] { new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()) },
                    "GfYVqdtZUJQfghRiaonAeRQRDjytRi47",
                    Debugger.IsAttached ? "http://localhost/" : "https://greenhand.azurewebsites.net",
                    Debugger.IsAttached ? "http://localhost/" : "https://greenhand.azurewebsites.net",
                    TimeSpan.FromHours(6));

                return Ok(token.RawData);
            }
                
            return Unauthorized();
        }



        //private string GetSiteUrl()
        //{
        //    //var settings = this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();



        //    if (string.IsNullOrEmpty(settings.HostName))
        //    {
        //        return "http://localhost";
        //    }
        //    else
        //    {
        //        return "https://" + settings.HostName + "/";
        //    }
        //}

        //private string GetSigningKey()
        //{
        //    //var settings = this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();

        //    if (string.IsNullOrEmpty(settings.HostName))
        //    {
        //        // this key is for debuggint and testing purposes only
        //        // this key should match the one supplied in Startup.MobileApp.cs
        //        return "GfYVqdtZUJQfghRiaonAeRQRDjytRi47";
        //    }
        //    else
        //    {
        //        return Environment.GetEnvironmentVariable("WEBSITE_AUTH_SIGNING_KEY");
        //    }
        //}
    }
}
