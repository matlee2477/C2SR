namespace C2SR.Services
{
    class ChangeTotalScoreRequestedEventArgs : EventArgs
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

    delegate void ChangeTotalScoreRequestedEventHandler(object sender, ChangeTotalScoreRequestedEventArgs e);
}
