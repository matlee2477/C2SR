using C2SR.Converters;
using System.Numerics;

namespace C2SR.Models
{
    public class C2Song
    {
        public C2Song(BigInteger id, string name, string artist, decimal bpm, C2SongVersion version, string chapter, string chartType, decimal level, decimal levelConstant)
        {
            ID = id;
            Name = name;
            Artist = artist;
            Bpm = bpm;
            Version = version;
            Chapter = chapter;
            ChartType = chartType;
            Level = level;
            LevelConstant = levelConstant;
            IsMM = false;
            TP = 0;
            IsMxm = false;
        }

        // Properties
        public BigInteger ID { get; }
        public string Name { get; }
        public string Artist { get; }
        public decimal Bpm { get; }
        public C2SongVersion Version { get; }
        public string Chapter { get; }
        public string ChartType { get; }
        public decimal Level { get; }
        public decimal LevelConstant { get; }

        public bool IsMM { get; set; }
        public decimal TP { get; set; }
        public bool IsMxm { get; set; }

        public decimal Score => ScoreConverter.GetScore(LevelConstant, IsMM, TP);
    }
}
