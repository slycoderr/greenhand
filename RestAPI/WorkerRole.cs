using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace RestAPI
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private IDisposable app;

        public override void Run()
        {
            //Trace.TraceInformation("RestAPI is running");
            File.AppendAllLines("info.txt", new List<string> { $"running" });
            try
            {
                RunAsync(cancellationTokenSource.Token).Wait();
            }
            finally
            {
                runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            var endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["https"];
            string baseUri = $"{endpoint.Protocol}://{endpoint.IPEndpoint}";

            //Trace.TraceInformation("RestAPI has been started");

            File.AppendAllLines("info.txt", new List<string> { $"{DateTime.Now:G} Connected at IP : {baseUri}" });

            app = WebApp.Start<Startup>(new StartOptions(baseUri));

            File.AppendAllLines("info.txt", new List<string> { $"started web app" });

            return base.OnStart();
        }

        public override void OnStop()
        {
            //Trace.TraceInformation("RestAPI is stopping");
            File.AppendAllLines("info.txt", new List<string> { $"stopping" });
            cancellationTokenSource.Cancel();
            runCompleteEvent.WaitOne();

            app?.Dispose();

            base.OnStop();

            //Trace.TraceInformation("RestAPI has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //Trace.TraceInformation("Working");
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}