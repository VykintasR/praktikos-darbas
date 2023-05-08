using System.Net;
using Bezdzione.Request;

namespace Bezdzione.Logs
{
    public static class MessageFormatter
    {
        private static string dateTime = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        public static string Info(string message)
        {
            return $"{dateTime} INFO: {message}{Environment.NewLine}";
        }
        public static string Error(string message)
        {
            return $"{dateTime} ERROR: {message}{Environment.NewLine}";
        }

        public static string RequestInfo(RequestParameters parameters)
        {
            return $"Trying to deploy server with region: {parameters.RegionSlug}, plan: {parameters.PlanSlug}, image: {parameters.ImageSlug}";
        }

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
