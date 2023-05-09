using Bezdzione;
using Bezdzione.Logs;
using Bezdzione.Request;
using RestSharp;
using System.Diagnostics;
using System.Reflection;

namespace BezdzioneTests
{
    [TestFixture]
    public  class ServerTests
    {
        [Test]
        public async Task TestDefaultServerDeployment()
        {
            // Deploy server
            Server server = new Server();
            int serverId = server.Deploy();

            string parameterInfo = $"region: {server.Parameters.RegionSlug}, plan: {server.Parameters.PlanSlug}, OS image: {server.Parameters.ImageSlug}";
            switch (serverId)
            {
                case > 0:
                    Console.WriteLine(MessageFormatter.Info(MessageFormatter.RequestInfo(server.Parameters)));

                    break;
                default:
                    Assert.Fail(MessageFormatter.Error($"Failed to deploy server with {parameterInfo}."));
                    break;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //Check server state
            string serverState = server.GetState();
            DateTime startTime = DateTime.Now;

            while (serverState != "active")
            {
                if ((DateTime.Now - startTime) > TimeSpan.FromMinutes(11))
                {
                    // if the maximum wait format is exceeded, log an error and fail the test
                    FileLogger.Log(MessageFormatter.Info($"Timeout - server took more than 10mins to become active."));
                    server.UpdateInfo();
                    server.Delete();
                    Assert.Fail("Timeout - server took more than 11mins to become active.");
                }

                await Task.Delay(5000); // Wait 5 seconds before checking server state again
                serverState = server.GetState();
            }

            stopwatch.Stop();
            server.UpdateInfo();

            //Assert that server became active within 10 minutes
            FileLogger.Log(MessageFormatter.Info($"Server took {stopwatch.Elapsed} to become active"));
            Assert.That(stopwatch.Elapsed, Is.LessThan(TimeSpan.FromMinutes(11)), $"Server took {stopwatch.Elapsed} to become active");

            await Task.Delay(15000);
            RestResponse response = server.Delete();
            FileLogger.Log(response.IsSuccessStatusCode ? MessageFormatter.Info("Server successfully deleted.") : MessageFormatter.Error("Failed to delete the server"));
        }

        public async Task TestRandomServerDeployment()
        {   
            // Deploy server
            Server server = new Server(new RandomParameterGenerator().GetRandomParameters());
            int serverId = server.Deploy();

            string parameterInfo = $"region: {server.Parameters.RegionSlug}, plan: {server.Parameters.PlanSlug}, OS image: {server.Parameters.ImageSlug}";
            switch (serverId)
            {
                case > 0:
                    Console.WriteLine(MessageFormatter.Info(MessageFormatter.RequestInfo(server.Parameters)));

                    break;
                default:
                    Assert.Fail(MessageFormatter.Error($"Failed to deploy server with {parameterInfo}."));
                    break;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //Check server state
            string serverState = server.GetState();
            DateTime startTime = DateTime.Now;

            while (serverState != "active")
            {
                if ((DateTime.Now - startTime) > TimeSpan.FromMinutes(11))
                {
                    // if the maximum wait format is exceeded, log an error and fail the test
                    FileLogger.Log(MessageFormatter.Info($"Timeout - server took more than 10mins to become active."));
                    server.UpdateInfo();
                    server.Delete();
                    Assert.Fail("Timeout - server took more than 11mins to become active.");
                }

                await Task.Delay(5000); // Wait 5 seconds before checking server state again
                serverState = server.GetState();
            }

            stopwatch.Stop();
            server.UpdateInfo();

            //Assert that server became active within 10 minutes
            FileLogger.Log(MessageFormatter.Info($"Server took {stopwatch.Elapsed} to become active"));
            Assert.That(stopwatch.Elapsed, Is.LessThan(TimeSpan.FromMinutes(11)), $"Server took {stopwatch.Elapsed} to become active");

            await Task.Delay(15000);
            RestResponse response = server.Delete();
            FileLogger.Log(response.IsSuccessStatusCode ? MessageFormatter.Info("Server successfully deleted.") : MessageFormatter.Error("Failed to delete the server"));
        }
    }
}
