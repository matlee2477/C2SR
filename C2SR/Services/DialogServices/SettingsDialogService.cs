using C2SR.Views;

namespace C2SR.Services.DialogServices
{
    class SettingsDialogService : DialogService<QuerySaveChangesDialogServiceInput, SettingsDialogServiceOutput>
    {
        public SettingsDialogService() : base() { }

        public override bool? ShowDialog(QuerySaveChangesDialogServiceInput input, out SettingsDialogServiceOutput output)
        {
            try
            {
                SettingsDialog dialog = new() { Owner = Owner };
                var result = dialog.ShowDialog();
                if (result == true)
                {
                    output = new()
                    {
                        Language = (C2Language)dialog.LanguageSetting,
                        StartAction = (C2StartAction)dialog.StartActionSetting,
                        HighlightsOutlyingLevelConstants = dialog.HighlightsOutlyingLevelConstants,
                        HighlightsBossSongs = dialog.HighlightsBossSongs,
                        HighlightsTopSongs = dialog.HighlightsTopSongs,
                        CascadesAchievements = dialog.CascadesAchievements,
                    };
                }
                else
                {
                    output = null!;
                }

                return result;
            }
            catch
            {
                output = null!;
                return null;
            }
        }
    }

    record SettingsDialogServiceInput : IDialogServiceInput
    {
        // The SettingsDialogServiceInput class does not require any input parameters.
    }

    record SettingsDialogServiceOutput : IDialogServiceOutput
    {
        public required C2Language Language { get; init; }
        public required C2StartAction StartAction { get; init; }
        public required bool HighlightsOutlyingLevelConstants { get; init; }
        public required bool HighlightsBossSongs { get; init; }
        public required bool HighlightsTopSongs { get; init; }
        public required bool CascadesAchievements { get; init; }
    }

}
