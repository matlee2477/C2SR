using C2SR.Models;
using C2SR.ViewModels;
using System.IO;
using System.Numerics;
using System.Text.Json.Nodes;

namespace C2SR.Services.JsonServices
{
    class SongDataJsonService : IJsonService<IEnumerable<C2SongViewModel>>
    {
        // Fields
        public int LevelThreshold { get; set; } = 0;

        // Methods
        public IEnumerable<C2SongViewModel> Load(string fileName)
        {
            using StreamReader sr = new(fileName);
            string code = sr.ReadToEnd();

            List<C2SongViewModel> songs = [];
            JsonArray arr = JsonNode.Parse(code)!.AsArray();
            foreach (JsonNode node in arr.OfType<JsonNode>())
            {
                if (node is not JsonObject obj) continue;

                BigInteger id = new(Convert.FromHexString(obj["ID"]!.GetValue<string>()));
                string name = obj["name"]?.GetValue<string>() ?? string.Empty;
                string artist = obj["artist"]?.GetValue<string>() ?? string.Empty;
                decimal bpm = obj["BPM"]?.GetValue<decimal>() ?? 0;
                string versionString = obj["version"]?.GetValue<string>() ?? string.Empty;
                string chapter = obj["chapter"]?.GetValue<string>() ?? string.Empty;
                string chartType = obj["chart"]?.GetValue<string>() ?? string.Empty;
                decimal level = obj["level"]?.GetValue<decimal>() ?? 12;
                decimal levelConstant = obj["const"]?.GetValue<decimal>() ?? level;

                if (level < LevelThreshold) continue;

                if (!C2SongVersion.TryParse(versionString, out C2SongVersion version))
                {
                    version = C2SongVersion.Empty;
                }

                C2SongViewModel song = new(new(id, name, artist, bpm, version, chapter, chartType, level, levelConstant));
                songs.Add(song);
            }

            return songs;
        }

        public void Save(string fileName, IEnumerable<C2SongViewModel> records)
        {
            // Saving song data is not available for the application
            throw new NotSupportedException();
        }
    }
}
