using Bezdzione;
using Bezdzione.Logs;
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
            RestResponse response = server.Deploy();

            //Check the time for server to become active
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //Check server state
            string serverState = server.GetState();
            DateTime startTime = DateTime.Now; // get the current time
            while (serverState != "active")
            {
                if ((DateTime.Now - startTime) > TimeSpan.FromMinutes(10)) // check if 10 minutes have elapsed
                {
                    // if the maximum wait time is exceeded, log an error and fail the test
                    Logger.LogError($"Server took too long to become active: {serverState}");
                    Assert.Fail("Server took too long to become active.");
                }

                await Task.Delay(5000); // Wait 10 seconds before checking server state again
                serverState = server.GetState();
            }
            stopwatch.Stop();

            //Get the active server image
            server.CreatedImage = server.CheckImageAfterActivation();


            //Assert and log results
            if (response.IsSuccessStatusCode)
            {
                Logger.LogInfo(MessageFormatter.FormatServerResponse(server, response));
                Logger.LogInfo($"Server took {stopwatch.Elapsed} to become active");
                // Assert that server became active within 10 minutes
                Assert.IsTrue(stopwatch.Elapsed < TimeSpan.FromMinutes(10), $"Server took {stopwatch.Elapsed} to become active");
            }
            else
            {
                Logger.LogError(MessageFormatter.FormatServerResponse(server, response));
                Assert.Fail($"Server failed to deploy.");
            }
        }
    }
}
