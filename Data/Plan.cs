using Bezdzione.Request;
using Newtonsoft.Json;
using RestSharp;

namespace Bezdzione.Data
{
    public class Plan
    {
        public int Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        [JsonProperty("category")]
        public string? Category { get; set; }
        [JsonProperty("available_regions")]
        public List<Region>? Regions { get; set; }
        [JsonIgnore]
        public List<Image>? Images 
        { 
            get
            {
                RestResponse response = HTTPClient.GetPlanImages(Slug);
                return response.Content != null ? JsonConvert.DeserializeObject<List<Image>>(response.Content) : new List<Image>();
            }
        }
    }   
}
