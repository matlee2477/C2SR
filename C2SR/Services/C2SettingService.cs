namespace C2SR.Services
{
    class C2SettingService : ISettingService
    {
        public C2SettingService()
        {
            Language = C2Language.English;
            StartAction = C2StartAction.NewDocument;
            LastFileName = string.Empty;
        }

        // Properties
        public C2Language Language { get; set; }
        public C2StartAction StartAction { get; set; }
        public string LastFileName { get; set; }

        // Singleton
        static readonly Lazy<C2SettingService> lazy = new(() => new C2SettingService());
        public static C2SettingService Instance => lazy.Value;
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
