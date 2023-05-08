namespace Bezdzione.Constants
{
    public static class API_URLS
    {
        public static string BASE_API_URL = "https://api.cherryservers.com/";
        public static string GetRegions = "v1/regions";
        public static string GetPlans = "v1/teams/" + Configuration.GetSetting("TEAM_ID") + "/plans";
        public static string GetPlanImages = GetPlans + "/{0}" + "/images";
        public static string RequestServer = "v1/projects/" + Configuration.GetSetting("PROJECT_ID") + "/servers";
        public static string RetrieveServerInfo = "v1/servers/" + "{0}";
        public static string DeleteServer = "v1/servers/" + "{0}";
    }
}
