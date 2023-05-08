using Bezdzione.Constants;
using Bezdzione.Request;
using Newtonsoft.Json;
using RestSharp;
namespace Bezdzione.Data
{
    public class RegionList
    {
        public List<Region>? Regions { get; }

        public RegionList()
        {
            Regions = new List<Region>();
        }

        private RegionList(string jsonContent)
        {
            if (jsonContent != null)
            {
                var deserializedPlans = JsonConvert.DeserializeObject<List<Region>>(jsonContent);
                Regions = deserializedPlans ?? new List<Region>();
            }
            else
            {
                Regions = new List<Region>();
            }
        }

        public static RegionList GetAllRegions()
        {
            RestResponse response = HTTPClient.GetRegions();
            string? content = response.Content;

            return content == null ? new RegionList() : new RegionList(content);
        }
    }
}
