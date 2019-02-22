using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cineworld.Api.Model
{
    internal class BookingsBody
    {
        [JsonProperty("events")]
        public List<Booking> Bookings { get; set; }
    }

    internal class BookingsResponse
    {
        [JsonProperty("body")]
        public BookingsBody Body { get; set; }
    }
}
