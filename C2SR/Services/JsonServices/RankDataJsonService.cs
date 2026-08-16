using C2SR.ViewModels;
using System.IO;
using System.Text.Json.Nodes;
using System.Windows.Media;

namespace C2SR.Services.JsonServices
{
    class RankDataJsonService : IJsonService<IEnumerable<C2TotalScoreRank>>
    {
        // Properties
        public IEnumerable<C2SongViewModel> Songs { get; set; } = [];
        public int TotalScoreSongCount { get; set; } = 0;

        // Methods
        public IEnumerable<C2TotalScoreRank> Load(string fileName)
        {
            using StreamReader sr = new(fileName);
            string code = sr.ReadToEnd();

            var ranks = new List<C2TotalScoreRank>();
            JsonArray arr = JsonNode.Parse(code)!.AsArray();
            foreach (JsonNode node in arr.OfType<JsonNode>())
            {
                if (node is not JsonObject obj) continue;

                if (obj.ContainsKey("top"))
                {
                    // Top criterion
                    string name = obj["top"]!.GetValue<string>();
                    byte r = obj["r"]?.GetValue<byte>() ?? 0;
                    byte g = obj["g"]?.GetValue<byte>() ?? 0;
                    byte b = obj["b"]?.GetValue<byte>() ?? 0;
                    Color color = new() { A = 255, R = r, G = g, B = b };

                    // Calculate top score
                    var topSongs = Songs.OrderByDescending(s => s.LevelConstant).Take(TotalScoreSongCount);
                    var topSongsWithTP100 = topSongs.Select(s =>
                    {
                        C2SongViewModel newSong = new(new(s.ID, s.Name, s.Artist, s.Bpm, s.Version, s.Chapter, s.ChartType, s.Level, s.LevelConstant));
                        newSong.IsMM = true;
                        newSong.TP = 100;
                        return newSong;
                    });
                    var result = TotalScoreService.GetTopSongs(topSongsWithTP100);

                    ranks.Add(new()
                    {
                        Name = name,
                        Criterion = result.TotalScore,
                        Color = color
                    });
                }
                else
                {
                    // Normal criterion
                    string name = obj[$"{Thread.CurrentThread.CurrentUICulture.Name}"]?.GetValue<string>() ?? string.Empty;
                    decimal score = obj["score"]?.GetValue<decimal>() ?? 100;
                    byte r = obj["r"]?.GetValue<byte>() ?? 0;
                    byte g = obj["g"]?.GetValue<byte>() ?? 0;
                    byte b = obj["b"]?.GetValue<byte>() ?? 0;
                    Color color = Color.FromRgb(r, g, b);

                    ranks.Add(new()
                    {
                        Name = name,
                        Criterion = score,
                        Color = color
                    });
                }
            }

            return ranks;
        }

        public void Save(string fileName, IEnumerable<C2TotalScoreRank> records)
        {
            // Saving rank data is not available for the application
            throw new NotSupportedException();
        }
    }
}
