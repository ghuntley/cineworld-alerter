using System.Diagnostics;
using Newtonsoft.Json;

namespace Cineworld.Api.Model
{
    [DebuggerDisplay("Name: {" + nameof(DisplayName) + "}")]
    public class Cinema
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("groupId")]
        public string GroupId { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("bookingUrl")]
        public string BookingUrl { get; set; }

        [JsonProperty("blockOnlineSales")]
        public bool BlockOnlineSales { get; set; }

        [JsonProperty("blockOnlineSalesUntil")]
        public object BlockOnlineSalesUntil { get; set; }
    }
}