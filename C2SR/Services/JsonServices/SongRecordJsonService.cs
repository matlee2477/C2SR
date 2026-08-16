using C2SR.Models;
using System.IO;
using System.Text.Json;

namespace C2SR.Services.JsonServices
{
    class SongRecordJsonService : IJsonService<IEnumerable<C2SongRecord>>
    {
        // Fields
        static readonly JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = false,
            WriteIndented = false
        };

        // Methods
        public IEnumerable<C2SongRecord> Load(string fileName)
        {
            using StreamReader reader = new(fileName);
            string code = reader.ReadToEnd();

            var records = JsonSerializer.Deserialize<C2SongRecord[]>(code, options)
                          ?? throw new InvalidDataException($"Failed to deserialize JSON from file '{fileName}'.");
            return records;
        }

        public void Save(string fileName, IEnumerable<C2SongRecord> records)
        {
            string code = JsonSerializer.Serialize(records, options);
            using StreamWriter writer = File.CreateText(fileName);
            writer.Write(code);
        }
    }
}
