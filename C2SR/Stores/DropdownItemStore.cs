using C2SR.Models;
using C2SR.Services.JsonServices;
using static C2SR.App.Constants;

namespace C2SR.Stores
{
    class DropdownItemStore
    {
        public DropdownItemStore()
        {
            DropdownItemDataJsonService js = new();
            DropdownItemData data = js.Load(PATH_DROPDOWN_JSON);

            var versions = data.Versions.Select(x =>
            {
                if (C2SongVersion.TryParse(x, out C2SongVersion version))
                {
                    return version;
                }
                else
                {
                    return C2SongVersion.Empty;
                }
            });
            var chapters = data.Chapters;
            var chartTypes = data.ChartTypes;
            var levels = data.Levels;
            Versions = [C2SongVersion.Empty, .. versions];
            Chapters = [string.Empty, .. chapters];
            ChartTypes = [string.Empty, .. chartTypes];
            Levels = [.. levels];
        }

        // Properties
        public C2SongVersion[] Versions { get; }
        public string[] Chapters { get; }
        public string[] ChartTypes { get; }
        public decimal[] Levels { get; }

        // Singleton
        static readonly Lazy<DropdownItemStore> lazy = new(() => new DropdownItemStore());
        public static DropdownItemStore Instance => lazy.Value;
    }
}
