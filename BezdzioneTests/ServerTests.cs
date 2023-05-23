using Bezdzione.Request;
using Bezdzione.Data;
using Bezdzione.Logging;
using RestSharp;
using System.Diagnostics;
using Newtonsoft.Json;

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
            string parameterInfo = $"region: {Server.Parameters.RegionSlug}, plan: {Server.Parameters.PlanSlug}, OS image: {Server.Parameters.ImageSlug}";
            int timeout = Server.Parameters.Timeout;
            Result testResult = new(timeout, Server.Parameters);

            //Try to deploy given Server
            dynamic? deploymentResponse = Server.Deploy(testResult);

            if (deploymentResponse == null)
            {
                string message = $"Failed to get a response when trying to deploy Server with {parameterInfo}.";
                testResult.IsSuccessful = false;
                testResult.DeploymentMessage = message;
                testResult.ResponseData = JsonConvert.SerializeObject("\"HasResponse\":false");
                DatabaseConnector.SaveData(testResult);
                Assert.Fail(message);
            }
            else if (deploymentResponse.code >= 400)
            {
                string message = $"{deploymentResponse.message}";
                testResult.IsSuccessful = false;
                testResult.DeploymentMessage = deploymentResponse.message;
                testResult.ResponseData = JsonConvert.SerializeObject(deploymentResponse);
                DatabaseConnector.SaveData(testResult);
                Assert.Fail(message);
            }
            else
            {
                ConsoleLogger.RequestInfo(Server);
                TimeSpan testTime = await WaitForServerActiveState(timeout, testResult);
                await Task.Delay(10000);
                AssertServerActive(testResult, testTime, timeout);
            }
        }
        private async Task<TimeSpan> WaitForServerActiveState(int timeout, Result testResult)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string serverState = Server.GetState();
            DateTime startTime = DateTime.Now;
            while (serverState != "active")
            {
                if ((DateTime.Now - startTime) > TimeSpan.FromMinutes(timeout))
                {
                    stopwatch.Stop();
                    HandleServerTimeout(stopwatch.Elapsed, testResult, timeout);
                }

                await Task.Delay(1000);
                serverState = Server.GetState();
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        private void AssertServerActive(Result testResult, TimeSpan time, int timeout)
        {
            LogServerActivationTime(time); 
            testResult.IsSuccessful = true;
            testResult.TestTime = time;
            UpdateResult(testResult);
            SaveResultToDatabase(testResult);
            HandleServerDeletion();
            Assert.Pass($"Test passed. Server deployed in {time} when timeout was {timeout} mins.");
        }

        private void HandleServerTimeout(TimeSpan time, Result testResult, int timeout)
        {
            testResult.IsSuccessful = false;
            testResult.TestTime = time;
            string message = $"Timeout - Server took more than {timeout} mins to become active. Test time - {time}";
            FileLogger.Log(MessageFormatter.Info(message));
            UpdateResult(testResult);
            SaveResultToDatabase(testResult);
            HandleServerDeletion();
            Assert.Fail(message);
        }

        private void HandleServerDeletion()
        {
            RestResponse deletionResponse = Server.Delete();
            LogServerDeletionStatus(deletionResponse.IsSuccessStatusCode);
        }

        private void UpdateResult(Result testResult)
        {
            dynamic? updateResponse = Server.UpdateAndGetInfo();

            if (updateResponse != null)
            {
                testResult.ResponseData = JsonConvert.SerializeObject(updateResponse);
            }
        }

        private void SaveResultToDatabase(Result testResult)
        {
            DatabaseConnector.SaveData(testResult);
        }

        private static void LogServerActivationTime(TimeSpan elapsed)
        {
            ConsoleLogger.Log(MessageFormatter.Info($"Server took {elapsed} to become active"));
            FileLogger.Log(MessageFormatter.Info($"Server took {elapsed} to become active"));
        }

        private static void LogServerDeletionStatus(bool isSuccess)
        {
            ConsoleLogger.Log(isSuccess ? MessageFormatter.Info("Server successfully deleted.") : MessageFormatter.Error("Failed to delete the Server"));
            FileLogger.Log(isSuccess ? MessageFormatter.Info("Server successfully deleted.") : MessageFormatter.Error("Failed to delete the Server"));
        }
    }
}
