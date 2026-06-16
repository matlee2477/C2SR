using C2SR.Converters;

namespace C2SR.Models
{
    class C2Song
    {
        public C2Song(long id, string name, string artist, decimal bpm, string version, string chapter, string chartType, decimal level, decimal levelConstant)
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
        public long ID { get; }
        public string Name { get; }
        public string Artist { get; }
        public decimal Bpm { get; }
        public string Version { get; }
        public string Chapter { get; }
        public string ChartType { get; }
        public decimal Level { get; }
        public decimal LevelConstant { get; }

        public bool IsMM { get; set; }
        public decimal TP { get; set; }
        public bool IsMxm { get; set; }

        public decimal Score => C2ScoreConverter.GetScore(LevelConstant, IsMM, TP);
    }
}
