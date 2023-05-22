using Bezdzione.CLI;
using Bezdzione.Data;
using Newtonsoft.Json;
using Bezdzione.Logging;

namespace Bezdzione.Request
{
    public class RequestParameters
    {
        [JsonProperty("plan")]
        public string? PlanSlug { get; private set; }
        [JsonProperty("image")]
        public string? ImageSlug { get; private set; }
        [JsonProperty("region")]
        public string? RegionSlug { get; private set; }
        [JsonIgnore]
        public string? Category { get; private set; }
        [JsonIgnore]
        public int Timeout { get; private set; }

        private RequestParameters() 
        { 

        }

        public RequestParameters(int? userTimeout, PlanList allPlans)
        {
            SetDefaultParameters(userTimeout, allPlans);
        }

        public RequestParameters(string? regionSlug, string? planSlug, string imageSlug, string category, int timeout)
        {
            SetRegion(regionSlug);
            SetPlan(planSlug);
            SetImage(imageSlug);
            SetCategory(category);
            SetTimeout(timeout);
        }

        public static RequestParameters ParamatetersFromOptions(PlanList filteredPlans, Options options)
        {
            RequestParameters parameters = new RequestParameters();

            if (options.Plan != null)
            {
                if (filteredPlans.Plans != null)
                {
                    Plan plan = filteredPlans.Plans.ElementAt(0);

                    parameters.SetPlan(plan.Slug);
                    parameters.SetCategory(plan.Category);
                    parameters.SetTimeout(options.Timeout);

                    if (options.Region != null)
                    {
                        parameters.SetRegion(options.Region);
                    }
                    else
                    {
                        Region? randomRegion = RandomParameterGenerator.RandomRegionFromPlan(plan);

                        if (randomRegion != null)
                        {
                            parameters.SetRegion(randomRegion.Slug);
                        }
                        else
                        {
                            ExceptionHandler.Handle(new Exception($"Failed to find a single valid region of plan {plan.Slug}."));
                        }
                    }
                    if (options.Image != null)
                    {
                        parameters.SetImage(options.Image);
                    }
                    else
                    {
                        Image? randomImage = RandomParameterGenerator.RandomImage(plan);
                        if (randomImage != null)
                        {
                            parameters.SetImage(randomImage.Slug);
                        }
                        else
                        {
                            ExceptionHandler.Handle(new Exception($"Failed to find a single valid image of plan {plan.Slug}."));
                        }
                    }
                }
                else
                {
                    ExceptionHandler.Handle(new Exception($"The filtered plan list is null."));
                }
                return parameters;
            }
            else if (options.Region != null)
            {
                parameters.SetRegion(options.Region);

                Plan? plan = RandomParameterGenerator.RandomPlan(filteredPlans);
                if (plan != null)
                {
                    parameters.SetPlan(plan.Slug);
                    parameters.SetCategory(options.Category != null ? options.Category : plan.Category);
                    parameters.SetTimeout(options.Timeout);

                    if(options.Image != null)
                    {
                        parameters.SetImage(options.Image);
                    }
                    else
                    {
                        Image? randomImage = RandomParameterGenerator.RandomImage(plan);

                        if (randomImage != null)
                        {
                            parameters.SetImage(randomImage.Slug);
                        }
                        else
                        {
                            ExceptionHandler.Handle(new Exception($"Failed to find a single valid image for plan {plan} in region {options.Region}."));
                        }
                    }
                }
                else
                {
                    ExceptionHandler.Handle(new Exception($"Failed to find a single plan in region {options.Region}."));
                }
                return parameters;
            }
            else if (options.Category != null)
            {
                Plan? plan = RandomParameterGenerator.RandomPlan(filteredPlans);

                if (plan != null)
                {
                    parameters.SetPlan(plan.Slug);
                    parameters.SetCategory(options.Category);
                    parameters.SetTimeout(options.Timeout);

                    Region? randomRegion = RandomParameterGenerator.RandomRegionFromPlan(plan);
                    if (randomRegion != null)
                    {
                        parameters.SetRegion(randomRegion.Slug);
                    }
                    else
                    {
                        ExceptionHandler.Handle(new Exception($"Failed to find a single valid region of plan {plan.Slug}."));
                    }

                    if (options.Image != null)
                    {
                        parameters.SetImage(options.Image);
                    }
                    else
                    {
                        Image? randomImage = RandomParameterGenerator.RandomImage(plan);

                        if (randomImage != null)
                        {
                            parameters.SetImage(randomImage.Slug);
                        }
                        else
                        {
                            ExceptionHandler.Handle(new Exception($"Failed to find a single valid image for plan {plan} in region {options.Region}."));
                        }
                    }
                }
                else
                {
                    ExceptionHandler.Handle(new Exception($"Failed to find a single plan in category {options.Category}."));
                }
                return parameters;
            }
            else //Only image is given
            {
                Plan? plan = RandomParameterGenerator.RandomPlan(filteredPlans);
                if (plan != null)
                {
                    parameters.SetPlan(plan.Slug);
                    parameters.SetCategory(plan.Category);
                    parameters.SetTimeout(options.Timeout);

                    Region? randomRegion = RandomParameterGenerator.RandomRegionFromPlan(plan);
                    if (randomRegion != null)
                    {
                        parameters.SetRegion(randomRegion.Slug);
                    }
                    else
                    {
                        ExceptionHandler.Handle(new Exception($"Failed to find a single valid region of plan {plan.Slug}."));
                    }
                    parameters.SetImage(options.Image);
                }
                else
                {
                    ExceptionHandler.Handle(new Exception($"Failed to find a single plan with image {options.Image}."));
                }
                return parameters;
            }
        }

            
        public void SetDefaultParameters(int? userTimeout, PlanList allPlans)
        {
            Timeout = TimeoutManager.DEFAULT_TIMEOUT;
            RegionList AvailableRegions = RegionList.GetAllRegions();
            if (AvailableRegions.Regions != null)
            {
                SetRegion(AvailableRegions.Regions.ElementAt(0).Slug);
            }

            if (RegionSlug != null)
            {
                PlanList regionPlans = allPlans.GetPlansFromRegion(RegionSlug);

                if (regionPlans.Plans != null)
                {
                    Plan firstPlan = regionPlans.Plans.ElementAt(0);
                    SetPlan(firstPlan.Slug);
                    SetCategory(firstPlan.Category);
                    SetTimeout(userTimeout);

                    if (firstPlan.Images != null && firstPlan.Images.ElementAt(1) != null)
                    {
                        SetImage(firstPlan.Images.ElementAt(0).Slug);
                    }
                }
            }
        }

        private void SetRegion(string? regionSlug)
        {
            RegionSlug = regionSlug;
        }
        private void SetPlan(string? planSlug)
        {
            PlanSlug = planSlug;
        }
        private void SetImage(string? imageSlug)
        {
            ImageSlug = imageSlug;
        }

        private void SetTimeout(int? userTimeout)
        {
            if (Category != null)
            {
                Timeout = userTimeout != null ? TimeoutManager.GetTimeout(userTimeout, Category) : TimeoutManager.GetTimeout(null, Category);
            }
            else
            {
                Timeout = userTimeout != null
                    ? userTimeout < TimeoutManager.MINIMUM_TIMEOUT ? TimeoutManager.DEFAULT_TIMEOUT : userTimeout.Value
                    : TimeoutManager.DEFAULT_TIMEOUT;
            }         
        }

        private void SetCategory(string? category)
        {
           Category = category;
        }
    }
}
