using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cineworld.Api.Model
{
    internal class DatesBody
    {
        [JsonProperty("dates")]
        public List<DateTimeOffset> Dates { get; set; }
    }

    internal class FilmDatesResponse
    {
        [JsonProperty("body")]
        public DatesBody Body { get; set; }
    }
}
