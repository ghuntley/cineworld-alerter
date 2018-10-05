using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cineworld.Api.Model
{
    internal class CinemaFilmResponse
    {
        [JsonProperty("body")]
        public List<CinemaFilm> Films { get; set; }
    }
}
