using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace C2SR.Services.JsonServices
{
    class DropdownItemDataJsonService : IJsonService<DropdownItemData>
    {
        // Fields
        static readonly JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = false,
            WriteIndented = false
        };

        // Methods
        public DropdownItemData Load(string fileName)
        {
            using StreamReader reader = new(fileName);
            string code = reader.ReadToEnd();

            var data = JsonSerializer.Deserialize<DropdownItemData>(code, options);
            return data;
        }

        public void Save(string fileName, DropdownItemData records)
        {
            // Saving rank data is not available for the application
            throw new NotSupportedException();
        }
    }

    readonly struct DropdownItemData
    {
        [JsonPropertyName("version")]
        public string[] Versions { get; init; }

        [JsonPropertyName("chapter")]
        public string[] Chapters { get; init; }

        [JsonPropertyName("chart")]
        public string[] ChartTypes { get; init; }

        [JsonPropertyName("level")]
        public decimal[] Levels { get; init; }
    }
}
