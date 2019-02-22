using System;
using Newtonsoft.Json;

namespace Cineworld.Api.Model
{
    public class Booking
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("filmId")]
        public string FilmId { get; set; }

        [JsonProperty("cinemaId")]
        public string CinemaId { get; set; }

        [JsonProperty("businessDay")]
        public DateTimeOffset BusinessDay { get; set; }

        [JsonProperty("eventDateTime")]
        public DateTimeOffset BookingTime { get; set; }

        [JsonProperty("attributeIds")]
        public string[] AttributeIds { get; set; }

        [JsonProperty("bookingLink")]
        public string BookingLink { get; set; }

        [JsonProperty("soldOut")]
        public bool IsSoldOut { get; set; }
    }
}