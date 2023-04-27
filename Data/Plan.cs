﻿using Bezdzione.Constants;
using Newtonsoft.Json;
using RestSharp;

namespace Bezdzione.Data
{
    internal class Plan
    {
        public int Id { get; set; }
        public string? Slug { get; set; }
        public string? Category { get; set; }
        [JsonProperty("available_regions")]
        public List<Region>? Regions { get; set; }
        [JsonIgnore]
        public List<Image>? Images 
        { 
            get
            {
                RestResponse response =  HTTPClient.SendHTTPRequest(string.Format(API_URLS.GetPlanImages, Slug), Method.Get);
                return response.Content != null ? JsonConvert.DeserializeObject<List<Image>>(response.Content) : new List<Image>();
            }
        }

        public void ShowInfo()
        {
            Console.Write("Slug: " + Slug + " Category: " +  Category + " ");

            if (Regions != null)
            {
                Console.WriteLine("Available regions:");

                foreach (Region r in Regions)
                {
                    Console.WriteLine(r.Slug);
                }
            }
            if (Images != null)
            {
                Console.WriteLine("Available images:");
                foreach (Image i in Images)
                {
                    Console.WriteLine(i.Slug);
                }
                Console.WriteLine("");
            }       
        }
    }   
}
