namespace C2SR.Services
{
    class SettingService
    {
        public SettingService()
        {
            Language = C2Language.English;
            StartAction = C2StartAction.NewDocument;
            LastFileName = string.Empty;
            HighlightsOutlyingLevelConstants = true;
            HighlightsBossSongs = true;
            HighlightsTopSongs = true;
            CascadesAchievements = true;
        }

        // Properties
        public C2Language Language { get; set; }
        public C2StartAction StartAction { get; set; }
        public string LastFileName { get; set; }
        public bool HighlightsOutlyingLevelConstants { get; set; }
        public bool HighlightsBossSongs { get; set; }
        public bool HighlightsTopSongs { get; set; }
        public bool CascadesAchievements { get; set; }

        // Singleton
        static readonly Lazy<SettingService> lazy = new(() => new SettingService());
        public static SettingService Instance => lazy.Value;
    }

    enum C2Language
    {
        English,
        Korean,
        Japanese,
    }

    enum C2StartAction
    {
        NewDocument,
        OpenLastDocument,
    }
}
