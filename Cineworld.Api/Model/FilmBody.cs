using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cineworld.Api.Model
{
    internal class FilmBody
    {
        [JsonProperty("posters")]
        public List<FullFilm> Films{ get; set; }

        [JsonProperty("films")]
        public List<FullFilm> Films2 { get; set; }
    }
}