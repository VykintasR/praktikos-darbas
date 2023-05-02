using RestSharp;


namespace Bezdzione.Logs
{
    public static class MessageFormatter
    {
        public static string FormatServerRequest(Server server)
        {
            return $"Sending request for server with RegionSlug: {server.RegionSlug}, PlanSlug: {server.PlanSlug}, ImageSlug: {server.ImageSlug}";
        }

        public static string FormatServerResponse(Server server, RestResponse response)
        {
            return $"Received response for server with RegionSlug: {server.RegionSlug}, PlanSlug: {server.PlanSlug}, ImageSlug: {server.ImageSlug}. Status code: {response.StatusCode}";
        }
    }
}
