using System.IO;
using System.Text.Json.Nodes;
using System.Windows;

namespace C2SR.Models
{
    class C2Document
    {
        C2Document(string fileName)
        {
            FileName = fileName;
            IsSaved = true;
            Songs = [];
        }

        // Properties
        public string FileName { get; set; }
        public bool IsSaved { get; set; }
        public List<C2Song> Songs { get; }

        // Methods
        public void Initialize()
        {
            FileName = string.Empty;
            IsSaved = false;
            foreach (var song in Songs)
            {
                song.IsMM = false;
                song.TP = 0;
                song.IsMxm = false;
            }
        }

        public void Load(string fileName)
        {
            try
            {
                using FileStream fs = new(fileName, FileMode.Open, FileAccess.Read);
                using StreamReader reader = new(fs);
                string code = reader.ReadToEnd();

                FileName = fileName;
                IsSaved = true;
                foreach (var song in Songs)
                {
                    song.IsMM = false;
                    song.TP = 0;
                    song.IsMxm = false;
                }

                JsonArray arr = JsonNode.Parse(code)!.AsArray();
                foreach (JsonObject obj in arr.OfType<JsonObject>())
                {
                    long id = obj["ID"]!.GetValue<long>();

                    C2Song? song = Songs.FirstOrDefault(s => s.ID == id);
                    if (song != null)
                    {
                        song.IsMM = obj["MM"]!.GetValue<bool>();
                        song.TP = obj["TP"]!.GetValue<decimal>();
                        song.IsMxm = obj["MxM"]!.GetValue<bool>();
                    }
                }
            }
            catch
            {
                MessageBox.Show("An error occurred while loading the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Initialize();
            }
        }

        public void Save(string fileName)
        {
            try
            {
                JsonArray arr = [];
                foreach (var song in Songs)
                {
                    JsonObject obj = new()
                    {
                        ["name"] = song.Name,
                        ["artist"] = song.Artist,
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
                MessageBox.Show("An error occurred while saving the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Singleton
        static readonly Lazy<C2Document> lazy = new(() => new(""));
        public static C2Document Instance => lazy.Value;
    }
}
