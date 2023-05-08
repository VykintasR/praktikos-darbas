using Bezdzione;
using Bezdzione.Logs;
using Bezdzione.Request;
using RestSharp;
using System.Diagnostics;

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

            if (serverId < 0)
            {
                Assert.Fail("Failed to deploy the server.");
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //Check server state
            string serverState = server.GetState();
            DateTime startTime = DateTime.Now;

            while (serverState != "active")
            {
                if ((DateTime.Now - startTime) > TimeSpan.FromMinutes(10))
                {
                    // if the maximum wait dateTime is exceeded, log an error and fail the test
                    FileLogger.Log(MessageFormatter.Error("Server took too long to become active."));
                    server.UpdateInfo();
                    RestResponse timeoutDelete = server.Delete();
                    Assert.Fail("Server took too long to become active.");
                }

                await Task.Delay(5000); // Wait 5 seconds before checking server state again
                serverState = server.GetState();
            }

            stopwatch.Stop();
            server.UpdateInfo();

            FileLogger.Log(MessageFormatter.Info($"Server took {stopwatch.Elapsed} to become active"));

            await Task.Delay(15000);
            RestResponse response = server.Delete();
            FileLogger.Log(response.IsSuccessStatusCode ? MessageFormatter.Info("Server successfully deleted.") : MessageFormatter.Error("Failed to delete the server"));
            Assert.Multiple(() =>
            {
                //Assert that server became active within 10 minutes
                Assert.That(stopwatch.Elapsed, Is.LessThan(TimeSpan.FromMinutes(10)), $"Server took {stopwatch.Elapsed} to become active");
                Assert.That(server.Id, Is.GreaterThan(0), "Failed to deploy the server.");
            });
        }

        public async Task TestRandomServerDeployment()
        {
            Server server = new Server(new RandomParameterGenerator().GetRandomParameters());
            int serverId = server.Deploy();

            if (serverId < 0)
            {
                Assert.Fail("Failed to deploy the server.");
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //Check server state
            string serverState = server.GetState();
            DateTime startTime = DateTime.Now;

            while (serverState != "active")
            {
                if ((DateTime.Now - startTime) > TimeSpan.FromMinutes(10))
                {
                    // if the maximum wait dateTime is exceeded, log an error and fail the test
                    FileLogger.Log(MessageFormatter.Error("Server took too long to become active."));
                    server.UpdateInfo();
                    RestResponse timeoutDelete = server.Delete();
                    Assert.Fail("Server took too long to become active.");
                }

                await Task.Delay(5000); // Wait 5 seconds before checking server state again
                serverState = server.GetState();
            }

            stopwatch.Stop();
            server.UpdateInfo();

            FileLogger.Log(MessageFormatter.Info($"Server took {stopwatch.Elapsed} to become active"));

            await Task.Delay(15000);
            RestResponse response = server.Delete();
            FileLogger.Log(response.IsSuccessStatusCode ? MessageFormatter.Info("Server successfully deleted.") : MessageFormatter.Error("Failed to delete the server"));
            Assert.Multiple(() =>
            {
                //Assert that server became active within 10 minutes
                Assert.That(stopwatch.Elapsed, Is.LessThan(TimeSpan.FromMinutes(10)), $"Server took {stopwatch.Elapsed} to become active");
                Assert.That(server.Id, Is.GreaterThan(0), "Failed to deploy the server in time.");
            });
        }
    }
}
