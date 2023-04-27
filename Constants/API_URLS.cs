namespace Bezdzione.Constants
{
    internal static class API_URLS
    {
        public static string BASE_API_URL = "https://api.cherryservers.com/";
        public static string GetRegions = "v1/regions";
        public static string GetPlans = "v1/teams/" + Configuration.GetSetting("TEAM_ID") + "/plans";
        public static string GetPlanImages = GetPlans + "/{0}" + "/images";
    }
}
