using C2SR.ViewModels;
using System.Windows.Media;
using static C2SR.App.Constants;

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
        public static C2TopSongResult GetTopSongs(IEnumerable<C2SongViewModel> songs)
        {
            List<C2SongViewModel> topSongs = [];
            decimal totalScore = 0;
            int count = 0;
            foreach (var song in songs.OrderByDescending(s => s.Score))
            {
                if (count < TOTAL_SCORE_SONG_COUNT && song.Score > 0)
                {
                    topSongs.Add(song);
                    totalScore += song.Score;
                    count++;
                }
            }

            return new()
            {
                TopSongs = [.. topSongs],
                TotalScore = totalScore,
                TopSongCount = count,
                IsUnranked = count < TOTAL_SCORE_SONG_COUNT
            };
        }

        public C2TotalScoreRank[] GetAllRanks() => [.. ranks];

        public void AddRank(string name, decimal criterion, Color color)
        {
            ranks.Add(new() { Name = name, Criterion = criterion, Color = color });
            ranks.Sort((a, b) => b.Criterion.CompareTo(a.Criterion));
        }

        public C2TotalScoreRank GetRankFromTotalScore(decimal totalScore)
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

    readonly struct C2TopSongResult
    {
        public C2SongViewModel[] TopSongs { get; init; }
        public decimal TotalScore { get; init; }
        public int TopSongCount { get; init; }
        public bool IsUnranked { get; init; }
    }

    readonly struct C2TotalScoreRank
    {
        public string Name { get; init; }
        public decimal Criterion { get; init; }
        public Color Color { get; init; }

        public override string ToString() => $"{Name}";
    }
}
