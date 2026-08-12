using C2SR.Models;
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

            var versions = obj["version"]!.AsArray().Select(x =>
            {
                if (C2SongVersion.TryParse(x?.GetValue<string>() ?? string.Empty, out C2SongVersion version))
                {
                    return version;
                }
                else
                {
                    return C2SongVersion.Empty;
                }
            });
            var chapters = obj["chapter"]!.AsArray().Select(x => x?.GetValue<string>() ?? string.Empty);
            var chartTypes = obj["chart"]!.AsArray().Select(x => x?.GetValue<string>() ?? string.Empty);
            var levels = obj["level"]!.AsArray().Select(x => x?.GetValue<decimal>() ?? 0);
            versions = versions.Prepend(C2SongVersion.Empty);
            chapters = chapters.Prepend(string.Empty);
            chartTypes = chartTypes.Prepend(string.Empty);
            Versions = [.. versions];
            Chapters = [.. chapters];
            ChartTypes = [.. chartTypes];
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
