using C2SR.Resources;
using C2SR.Services.JsonServices;
using System.Text.Json.Nodes;
using static C2SR.App.Constants;

namespace C2SR.Stores
{
    class DropdownItemStore
    {
        public DropdownItemStore()
        {
            JsonService jsonService = new();
            string code = jsonService.LoadJson(PATH_DROPDOWN_JSON);
            JsonObject obj = JsonNode.Parse(code)!.AsObject();

            var versions = obj["version"]!.AsArray().Select(x => x?.GetValue<string>() ?? string.Empty);
            var chapters = obj["chapter"]!.AsArray().Select(x => x?.GetValue<string>() ?? string.Empty);
            var chartTypes = obj["chart"]!.AsArray().Select(x => x?.GetValue<string>() ?? string.Empty);
            var levels = obj["level"]!.AsArray().Select(x => x?.GetValue<decimal>() ?? 0);
            versions = versions.Prepend(Strings.MainWindow_Filters_All);
            chapters = chapters.Prepend(Strings.MainWindow_Filters_All);
            chartTypes = chartTypes.Prepend(Strings.MainWindow_Filters_All);
            Versions = [.. versions];
            Chapters = [.. chapters];
            ChartTypes = [.. chartTypes];
            Levels = [.. levels];
        }

        // Properties
        public string[] Versions { get; }
        public string[] Chapters { get; }
        public string[] ChartTypes { get; }
        public decimal[] Levels { get; }

        // Singleton
        static readonly Lazy<DropdownItemStore> lazy = new(() => new DropdownItemStore());
        public static DropdownItemStore Instance => lazy.Value;
    }
}
