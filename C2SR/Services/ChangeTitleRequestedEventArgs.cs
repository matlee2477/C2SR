namespace C2SR.Services
{
    class ChangeTitleRequestedEventArgs : EventArgs
    {
        public ChangeTitleRequestedEventArgs(string fileName, bool isSaved)
        {
            FileName = fileName;
            IsSaved = isSaved;
        }

        // Properties
        public string FileName { get; }
        public bool IsSaved { get; }
    }

    delegate void ChangeTitleRequestedEventHandler(object sender, ChangeTitleRequestedEventArgs e);
}
