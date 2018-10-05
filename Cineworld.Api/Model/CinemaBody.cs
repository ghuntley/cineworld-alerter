using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cineworld.Api.Model
{
    internal class CinemaBody
    {
        [JsonProperty("cinemas")]
        public List<Cinema> Cinemas { get; set; }
    }
}