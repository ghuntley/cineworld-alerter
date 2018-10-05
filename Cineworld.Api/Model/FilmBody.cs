using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cineworld.Api.Model
{
    internal class FilmBody
    {
        [JsonProperty("posters")]
        public List<FullFilm> Films{ get; set; }
    }
}