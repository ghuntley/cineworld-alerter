using Newtonsoft.Json;

namespace Cineworld.Api.Model
{
    internal class CinemaResponse
    {
        [JsonProperty("body")]
        public CinemaBody Body { get; set; }
    }
}
