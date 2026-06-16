using System.Windows.Media;

namespace C2SR.Services
{
    class C2TotalScoreService
    {
        C2TotalScoreService()
        {
            ranks = [];
        }

        // Fields
        readonly List<C2TotalScoreRank> ranks;

        // Methods
        public void AddRank(string name, decimal criterion, Color color)
        {
            ranks.Add(new() { Name = name, Criterion = criterion, Color = color });
            ranks.Sort((a, b) => b.Criterion.CompareTo(a.Criterion));
        }

        public C2TotalScoreRank GetRank(decimal totalScore)
        {
            foreach (var rank in ranks)
            {
                if (totalScore >= rank.Criterion)
                {
                    return rank;
                }
            }

            return new() { Name = string.Empty, Criterion = 0, Color = Colors.White };
        }

        // Singleton
        static readonly Lazy<C2TotalScoreService> lazy = new(() => new C2TotalScoreService());
        public static C2TotalScoreService Instance => lazy.Value;
    }

    readonly struct C2TotalScoreRank
    {
        public string Name { get; init; }
        public decimal Criterion { get; init; }
        public Color Color { get; init; }

        public override string ToString() => $"{Name}";
    }
}
