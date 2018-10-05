using Newtonsoft.Json;

namespace Cineworld.Api.Model
{
    public class CinemaFilm
    {
        [JsonProperty("filmId")]
        public string FilmId { get; set; }

        [JsonProperty("filmName")]
        public string FilmName { get; set; }

        [JsonProperty("filmLink")]
        public string FilmLink { get; set; }

        [JsonProperty("videoLink")]
        public string VideoLink { get; set; }
    }
}