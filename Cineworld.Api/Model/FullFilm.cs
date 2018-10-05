using Newtonsoft.Json;

namespace Cineworld.Api.Model
{
    public class FullFilm
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("attributes")]
        public string[] Attributes { get; set; }

        [JsonProperty("featureTitle")]
        public string FeatureTitle { get; set; }

        [JsonProperty("posterSrc")]
        public string PosterSrc { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonProperty("dateStarted")]
        public string DateStarted { get; set; }
    }
}