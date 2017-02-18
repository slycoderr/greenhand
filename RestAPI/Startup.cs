using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Owin;

namespace RestAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            File.AppendAllLines("info.txt", new List<string> { $"config start" });
            ConfigureOAuth(appBuilder);
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            config.Filters.Add(new HostAuthenticationFilter("Bearer"));
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            File.AppendAllLines("info.txt", new List<string> { "routes: "+config.Routes.Count });

            foreach (var r in config.Routes)
            {
                File.AppendAllLines("info.txt", new List<string> { r.RouteTemplate});
            }

            appBuilder.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            appBuilder.UseWebApi(config);

            File.AppendAllLines("info.txt", new List<string> { $"config end" });
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = false,
                TokenEndpointPath = new PathString("/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider(),
            };

            // Create Tokens
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}