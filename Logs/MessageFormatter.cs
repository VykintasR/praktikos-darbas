using RestSharp;
using System.Net;

namespace Bezdzione.Logs
{
    public static class MessageFormatter
    {
        public static string FormatServerRequest(Server server)
        {
            return $"Sending request for server with RequestRegionSlug: {server.RequestRegionSlug}, RequestPlanSlug: {server.RequestPlanSlug}, RequestImageSlug: {server.RequestImageSlug}";
        }

        public static string FormatServerResponse(Server server, RestResponse response)
        {
            string message = $"Response for Server with Status Code: {response.StatusCode}";

            if (response.StatusCode == HttpStatusCode.Created)
            {
                message += $", Server ID: {server.Id}";
                message += $", Server Name: {server.Name}";
                message += $", Region: {server.CreatedRegion}";
                message += $", Plan: {server.CreatedPlan}";
                message += $", OS Image: {server.CreatedImage}";
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                message += $", Error Message: {response.ErrorMessage}";
            }

            return message;
        }
    }
}
