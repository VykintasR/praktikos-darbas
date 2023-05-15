using System.Net;
using System.Threading;
using Bezdzione.Request;

namespace Bezdzione.Logs
{
    public static class MessageFormatter
    {
        public static string Info(string message) => $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} INFO: {message}{Environment.NewLine}";
        public static string Error(string message) => $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} ERROR: {message}{Environment.NewLine}";

        public static string RequestInfo(Parameters parameters) => $"Trying to deploy server in {parameters.Timeout} minutes with region: {parameters.RegionSlug}, plan: {parameters.PlanSlug}, image: {parameters.ImageSlug}";

        public static string ResponseInfo(dynamic response, HttpStatusCode status)
        {
            string message = $"Response for Server with Status Code: {status}";
            message += $", Server ID: {response.id}";
            message += $", Server Name: {response.hostname}";
            message += $", Region: {response.region.slug}";
            message += $", Plan: {response.name}";
            message += $", OS Image: {response.image}{Environment.NewLine}";
            return message;
        }
    }
}
