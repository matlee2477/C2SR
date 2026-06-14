using System.IO;
using System.Text.Json.Nodes;

namespace C2SR.Models
{
    class C2Document
    {
        C2Document(string fileName)
        {
            FileName = fileName;
            IsSaved = true;
        }

        // Properties
        public string FileName { get; set; }
        public bool IsSaved { get; set; }

        // Methods
        public C2FileData[] Load(string fileName)
        {
            try
            {
                using FileStream fs = new(fileName, FileMode.Open, FileAccess.Read);
                using StreamReader reader = new(fs);
                string code = reader.ReadToEnd();

                List<C2FileData> fileData = [];
                JsonArray arr = JsonNode.Parse(code)!.AsArray();
                foreach (JsonObject obj in arr.OfType<JsonObject>())
                {
                    long id = obj["ID"]!.GetValue<long>();
                    bool isMM = obj["MM"]!.GetValue<bool>();
                    decimal tp = obj["TP"]!.GetValue<decimal>();
                    bool isMxm = obj["MxM"]!.GetValue<bool>();
                    C2FileData data = new() { ID = id, IsMM = isMM, TP = tp, IsMxm = isMxm };
                    fileData.Add(data);
                }

                FileName = fileName;
                IsSaved = true;
                return [.. fileData];
            }
            catch
            {
                throw;
            }
        }

        public void Save(string fileName, C2Song[] songs)
        {
            try
            {
                JsonArray arr = [];
                foreach (var song in songs)
                {
                    JsonObject obj = new()
                    {
                        ["ID"] = song.ID,
                        ["MM"] = song.IsMM,
                        ["TP"] = song.TP,
                        ["MxM"] = song.IsMxm
                    };
                    arr.Add(obj);
                }

                using FileStream fs = new(fileName, FileMode.Create, FileAccess.Write);
                using StreamWriter writer = new(fs);
                writer.Write(arr.ToJsonString());

                FileName = fileName;
                IsSaved = true;
            }
            catch
            {
                throw;
            }
        }

        // Singleton
        static readonly Lazy<C2Document> lazy = new(() => new(""));
        public static C2Document Instance => lazy.Value;
    }
}
