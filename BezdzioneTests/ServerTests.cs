using Bezdzione.Logs;
using Bezdzione.Request;
using Bezdzione.Data;
using RestSharp;
using System.Diagnostics;

namespace BezdzioneTests
{
    [TestFixture]
    public class ServerTests
    {
        public Server Server { get; private set; }
        public Bezdzione.Request.RequestParameters GetServerParameters() => Server.Parameters;

        public ServerTests(PlanList allPlans) 
        {
            Server = new Server(allPlans);
        }

        [SetUp]
        public void SetUp(Server server)
        {
            Server = server;
        }

        [Test]
        public async Task TestServerDeployment()
        {
            // Deploy given Server
            int serverId = Server.Deploy();
            int timeout = Server.Parameters.Timeout;

            string parameterInfo = $"region: {Server.Parameters.RegionSlug}, plan: {Server.Parameters.PlanSlug}, OS image: {Server.Parameters.ImageSlug}";

            HandleServerDeployment(serverId, parameterInfo);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            await WaitForServerActiveState(timeout);

            stopwatch.Stop();
            Server.UpdateInfo();

            AssertServerActive(stopwatch.Elapsed, timeout);

            await Task.Delay(15000);
            RestResponse response = Server.Delete();
            LogServerDeletionStatus(response.IsSuccessStatusCode);
        }

        private void HandleServerDeployment(int serverId, string parameterInfo)
        {
            switch (serverId)
            {
                case > 0:
                    ConsoleLogger.RequestInfo(Server);
                    break;
                default:
                    Assert.Fail($"Failed to deploy Server with {parameterInfo}.");
                    break;
            }
        }
        private async Task WaitForServerActiveState(int timeout)
        {
            string serverState = Server.GetState();
            DateTime startTime = DateTime.Now;

            while (serverState != "active")
            {
                if ((DateTime.Now - startTime) > TimeSpan.FromMinutes(timeout))
                {
                    HandleServerTimeout(timeout);
                }

                await Task.Delay(5000);
                serverState = Server.GetState();
            }
        }

        private void HandleServerTimeout(int timeout)
        {
            FileLogger.Log(MessageFormatter.Info($"Timeout - Server took more than {timeout} mins to become active."));
            Server.UpdateInfo();
            Server.Delete();
            Assert.Fail($"Timeout - Server took more than {timeout} mins to become active.");
        }

        private void AssertServerActive(TimeSpan elapsed, int timeout)
        {
            ConsoleLogger.Log(MessageFormatter.Info($"Server took {elapsed} to become active"));
            FileLogger.Log(MessageFormatter.Info($"Server took {elapsed} to become active"));
            Assert.That(elapsed, Is.LessThan(TimeSpan.FromMinutes(timeout)), $"Timeout. Server took {elapsed} to become active. Maximum allowed time - {timeout} minutes.");
        }

        private void LogServerDeletionStatus(bool isSuccess)
        {
            ConsoleLogger.Log(isSuccess ? MessageFormatter.Info("Server successfully deleted.") : MessageFormatter.Error("Failed to delete the Server"));
            FileLogger.Log(isSuccess ? MessageFormatter.Info("Server successfully deleted.") : MessageFormatter.Error("Failed to delete the Server"));
        }
    }
}
