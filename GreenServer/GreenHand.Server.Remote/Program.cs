using System;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Owin;
using TelemeWiz;
using TelemeWiz.Common.Models;

namespace GreenHand.Server.Remote
{
    internal class Program
    {
        /// <summary>
        ///     determines what host address to host on. for local development localhost:9101
        ///     for lan development *:9101
        /// </summary>
        private static string DetermineBaseAddress()
        {
            const string testServerHost = "192.168.0.99";
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList[1];
            return testServerHost.Equals(ipAddress.ToString()) ? "https://*:9101" : "http://localhost:9101/";
        }

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            var baseAddress = DetermineBaseAddress();

            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine("Connected at IP : {0}", baseAddress);
                Console.WriteLine("Press Enter to Exit");
                Console.ReadLine();
            }
        }

        private static async void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            await Telemetry.Client.LogException((Exception) e.ExceptionObject);
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            appBuilder.UseWebApi(config);
        }
    }

    public static class Telemetry
    {
        public static readonly TelemetryClient Client = new TelemetryClient(new Metadata(Guid.NewGuid(), Environment.OSVersion.VersionString, typeof(Program).Assembly.GetName().Name, typeof(Program).Assembly.GetName().Version.ToString())){ServiceUrl = "http://localhost:6969"};
    }
}