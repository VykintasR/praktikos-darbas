using Bezdzione.Request;
using Newtonsoft.Json;

namespace Bezdzione.Logging
{
    public class Result
    {
        public string Category { get; private set; }
        public bool IsSuccessful { get; set; }
        public TimeSpan TestTime { get; set; }
        public int TimeoutMins { get; set; }
        public int HTTPStatusCode { get; set; }
        public string DeploymentMessage { get; set; } = string.Empty;
        public string RequestData { get; private set; } = string.Empty;
        public string ResponseData { get; set; } = string.Empty;

        public Result(int timeout, RequestParameters parameters)
        {
            TimeoutMins = timeout;
            Category = parameters.Category ?? "Unknown";
            RequestData = JsonConvert.SerializeObject(parameters);
        }

    }
}
