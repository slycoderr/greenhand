using System;
using System.Diagnostics;
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
            Trace.TraceInformation("RestAPI is running");

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

            var result = base.OnStart();
            var endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Endpoint1"];
            string baseUri = $"{endpoint.Protocol}://{endpoint.IPEndpoint}";

            Trace.TraceInformation("RestAPI has been started");

            using (WebApp.Start<Startup>(baseUri))
            {
                Console.WriteLine("Connected at IP : {0}", baseUri);
                Console.WriteLine("Press Enter to Exit");
                Console.ReadLine();
            }

            app = WebApp.Start<Startup>(new StartOptions(baseUri));

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("RestAPI is stopping");

            cancellationTokenSource.Cancel();
            runCompleteEvent.WaitOne();

            app?.Dispose();

            base.OnStop();

            Trace.TraceInformation("RestAPI has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}