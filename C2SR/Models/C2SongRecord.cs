using System.Text.Json.Serialization;

namespace C2SR.Models
{
    public record C2SongRecord
    {
        [JsonPropertyName("ID")]
        public required string ID { get; init; }

        [JsonPropertyName("MM")]
        public required bool IsMM { get; init; }

        [JsonPropertyName("TP")]
        public required decimal TP { get; init; }

        [JsonPropertyName("MxM")]
        public required bool IsMxm { get; init; }
    }
}
