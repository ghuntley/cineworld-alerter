using Newtonsoft.Json;

namespace Cineworld.Api.Model
{
    internal class FullFilmResponse
    {
        [JsonProperty("body")]
        public FilmBody Body { get; set; }
    }
}
