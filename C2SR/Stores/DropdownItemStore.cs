using C2SR.Resources;
using C2SR.Services.JsonServices;
using System.Text.Json.Nodes;

namespace C2SR.Stores
{
    class DropdownItemStore
    {
        public DropdownItemStore()
        {
            C2JsonService jsonService = new();
            string code = jsonService.LoadJson(@".\data\dropdownitems.json");
            JsonObject obj = JsonNode.Parse(code)!.AsObject();

            var versions = obj["version"]!.AsArray().Select(x => x?.GetValue<string>() ?? string.Empty);
            var chapters = obj["chapter"]!.AsArray().Select(x => x?.GetValue<string>() ?? string.Empty);
            var chartTypes = obj["chart"]!.AsArray().Select(x => x?.GetValue<string>() ?? string.Empty);
            var levels = obj["level"]!.AsArray().Select(x => (object)(x?.GetValue<decimal>() ?? 0));
            versions = versions.Prepend(Strings.MainWindow_Filters_All);
            chapters = chapters.Prepend(Strings.MainWindow_Filters_All);
            chartTypes = chartTypes.Prepend(Strings.MainWindow_Filters_All);
            levels = levels.Prepend(Strings.MainWindow_Filters_All);
            Versions = [.. versions];
            Chapters = [.. chapters];
            ChartTypes = [.. chartTypes];
            Levels = [.. levels];
        }

        // Properties
        public string[] Versions { get; }
        public string[] Chapters { get; }
        public string[] ChartTypes { get; }
        public object[] Levels { get; }

        // Singleton
        static readonly Lazy<DropdownItemStore> lazy = new(() => new DropdownItemStore());
        public static DropdownItemStore Instance => lazy.Value;
    }
}
