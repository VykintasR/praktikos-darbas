using Bezdzione.Logs;
using Bezdzione.Request;
using RestSharp;
using System.Diagnostics;

namespace BezdzioneTests
{
    [TestFixture]
    public class ServerTests
    {
        private Server server = new Server();

        [SetUp]
        public void SetUp(Server serverConfigurationToTest)
        {
            server = serverConfigurationToTest;
        }

        [Test]
        public async Task TestServerDeployment()
        {
            // Deploy given server
            int serverId = server.Deploy();
            int timeout = server.Parameters.Timeout;

            string parameterInfo = $"region: {server.Parameters.RegionSlug}, plan: {server.Parameters.PlanSlug}, OS image: {server.Parameters.ImageSlug}";

            HandleServerDeployment(serverId, parameterInfo);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            await WaitForServerActiveState(timeout);

            stopwatch.Stop();
            server.UpdateInfo();

            AssertServerActive(stopwatch.Elapsed, timeout);

            await Task.Delay(15000);
            RestResponse response = server.Delete();
            LogServerDeletionStatus(response.IsSuccessStatusCode);
        }

        private void HandleServerDeployment(int serverId, string parameterInfo)
        {
            switch (serverId)
            {
                case > 0:
                    ConsoleLogger.RequestInfo(server);
                    break;
                default:
                    Assert.Fail(MessageFormatter.Error($"Failed to deploy server with {parameterInfo}."));
                    break;
            }
        }
        private async Task WaitForServerActiveState(int timeout)
        {
            string serverState = server.GetState();
            DateTime startTime = DateTime.Now;

            while (serverState != "active")
            {
                if ((DateTime.Now - startTime) > TimeSpan.FromMinutes(timeout))
                {
                    HandleServerTimeout(timeout);
                }

                await Task.Delay(5000);
                serverState = server.GetState();
            }
        }

        private void HandleServerTimeout(int timeout)
        {
            FileLogger.Log(MessageFormatter.Info($"Timeout - server took more than {timeout} mins to become active."));
            server.UpdateInfo();
            server.Delete();
            Assert.Fail($"Timeout - server took more than {timeout} mins to become active.");
        }

        private void AssertServerActive(TimeSpan elapsed, int timeout)
        {
            ConsoleLogger.Log(MessageFormatter.Info($"Server took {elapsed} to become active"));
            FileLogger.Log(MessageFormatter.Info($"Server took {elapsed} to become active"));
            Assert.That(elapsed, Is.LessThan(TimeSpan.FromMinutes(timeout)), $"Timeout. Server took {elapsed} to become active. Maximum allowed time - {timeout} minutes.");
        }

        private void LogServerDeletionStatus(bool isSuccess)
        {
            ConsoleLogger.Log(isSuccess ? MessageFormatter.Info("Server successfully deleted.") : MessageFormatter.Error("Failed to delete the server"));
            FileLogger.Log(isSuccess ? MessageFormatter.Info("Server successfully deleted.") : MessageFormatter.Error("Failed to delete the server"));
        }
    }
}
