namespace C2SR.Models
{
    class C2Song
    {
        public C2Song(int id, string name, string artist, int chapter, int chartType, decimal level, decimal levelConstant)
        {
            ID = id;
            Name = name;
            Artist = artist;
            Chapter = chapter;
            ChartType = chartType;
            Level = level;
            LevelConstant = levelConstant;
            IsMM = false;
            TP = 0;
            IsMxm = false;
        }

        // Properties
        public int ID { get; }
        public string Name { get; }
        public string Artist { get; }
        public int Chapter { get; }
        public int ChartType { get; }
        public decimal Level { get; }
        public decimal LevelConstant { get; }

        public bool IsMM { get; set; }
        public decimal TP { get; set; }
        public bool IsMxm { get; set; }

        public decimal Rate
        {
            get
            {
                decimal rate = Math.Floor(TP * LevelConstant * 10) / 1000;
                if (IsMM) rate += 0.3m;
                if (TP == 100m) rate += 0.2m;
                return rate;
            }
        }
    }
}
