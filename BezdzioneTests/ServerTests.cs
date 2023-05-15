using Bezdzione;
using Bezdzione.Logs;
using Bezdzione.Request;
using RestSharp;
using System.Diagnostics;

namespace BezdzioneTests
{
    [TestFixture]
    public class ServerTests
    {
        private Server server;

        [SetUp]
        public void SetUp(Server serverConfigurationToTest)
        {
            server = serverConfigurationToTest;
        }

        [Test]
        [TestCase(3)]
        public async Task TestServerDeployment(int timeout)
        {
            // Deploy given server
            int serverId = server.Deploy();

            string parameterInfo = $"region: {server.Parameters.RegionSlug}, plan: {server.Parameters.PlanSlug}, OS image: {server.Parameters.ImageSlug}";
            switch (serverId)
            {
                case > 0:
                    ConsoleLogger.RequestInfo(server);
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
                if ((DateTime.Now - startTime) > TimeSpan.FromMinutes(timeout))
                {
                    // if the maximum wait format is exceeded, log an error and fail the test
                    FileLogger.Log(MessageFormatter.Info($"Timeout - server took more than {timeout} mins to become active."));
                    server.UpdateInfo();
                    server.Delete();
                    Assert.Fail($"Timeout - server took more than {timeout} mins to become active.");
                }

                await Task.Delay(5000); // Wait 5 seconds before checking server state again
                serverState = server.GetState();
            }

            stopwatch.Stop();
            server.UpdateInfo();

            //Assert that server became active within allowed time in minutes
            ConsoleLogger.Log(MessageFormatter.Info($"Server took {stopwatch.Elapsed} to become active"));
            FileLogger.Log(MessageFormatter.Info($"Server took {stopwatch.Elapsed} to become active"));
            Assert.That(stopwatch.Elapsed, Is.LessThan(TimeSpan.FromMinutes(timeout)), $"Timeout. Server took {stopwatch.Elapsed} to become active. Maximum allowed time - {timeout} minutes.");

            await Task.Delay(15000);
            RestResponse response = server.Delete();
            ConsoleLogger.Log(response.IsSuccessStatusCode ? MessageFormatter.Info("Server successfully deleted.") : MessageFormatter.Error("Failed to delete the server"));
            FileLogger.Log(response.IsSuccessStatusCode ? MessageFormatter.Info("Server successfully deleted.") : MessageFormatter.Error("Failed to delete the server"));
        }
    }
}
