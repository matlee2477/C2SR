namespace C2SR.EventHandling
{
    public class ChangeTotalScoreRequestedEventArgs : EventArgs
    {
        public ChangeTotalScoreRequestedEventArgs(decimal totalScore, bool isUnranked)
        {
            TotalScore = totalScore;
            IsUnranked = isUnranked;
        }

        // Properties
        public decimal TotalScore { get; }
        public bool IsUnranked { get; }
    }

    public delegate void ChangeTotalScoreRequestedEventHandler(object sender, ChangeTotalScoreRequestedEventArgs e);
}
