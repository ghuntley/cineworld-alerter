using System;
using System.Collections.Generic;
using Cineworld.Api.Converters;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Cineworld.Api.Model
{
    [DebuggerDisplay("Name: {" + nameof(FeatureTitle) + "}")]
    public class FullFilm
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("attributes", ItemConverterType = typeof(FilmCategoryConverter))]
        public List<FilmCategory> Attributes { get; set; }

        [JsonProperty("featureTitle")]
        public string FeatureTitle { get; set; }

        [JsonProperty("posterSrc")]
        public string PosterSrc { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonProperty("dateStarted")]
        public DateTimeOffset? DateStarted { get; set; }
    }
}