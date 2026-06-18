using C2SR.Resources;
using C2SR.ViewModels;
using C2SR.Views;
using Microsoft.Win32;
using System.Collections;
using System.Windows;

namespace C2SR.Services.DialogServices
{
    class C2DialogService : IDialogService
    {
        public C2DialogService(Window owner)
        {
            Owner = owner;
        }

        // Properties
        public Window Owner { get; set; }

        #region Methods
        public MessageBoxResult QuerySaveChangesDialog() => MessageBox.Show(Owner, Strings.MessageBox_QuerySaveChanges, Strings.Title,
            MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

        public bool ShowOpenFileDialog(out string fileName)
        {
            OpenFileDialog dialog = new()
            {
                Title = Strings.OpenFileDialog_Title,
                Filter = FILTER,
                DefaultExt = DEFAULT_EXT,
                AddExtension = true,
                Multiselect = false
            };

            if (dialog.ShowDialog(Owner) == true)
            {
                fileName = dialog.FileName;
                return true;
            }
            else
            {
                fileName = null!;
                return false;
            }
        }

        public bool ShowSaveFileDialog(out string fileName)
        {
            SaveFileDialog dialog = new()
            {
                Title = Strings.SaveFileDialog_Title,
                Filter = FILTER,
                DefaultExt = DEFAULT_EXT,
                AddExtension = true,
                OverwritePrompt = true
            };

            if (dialog.ShowDialog(Owner) == true)
            {
                fileName = dialog.FileName;
                return true;
            }
            else
            {
                fileName = null!;
                return false;
            }
        }

        public void ShowOpenErrorDialog() => MessageBox.Show(Owner, Strings.MessageBox_Error_Load, Strings.Title, MessageBoxButton.OK, MessageBoxImage.Error);
        public void ShowSaveErrorDialog() => MessageBox.Show(Owner, Strings.MessageBox_Error_Save, Strings.Title, MessageBoxButton.OK, MessageBoxImage.Error);

        public SettingsDialogResult ShowSettingsDialog()
        {
            SettingsDialog dialog = new() { Owner = Owner };
            if (dialog.ShowDialog() == true)
            {
                return new SettingsDialogResult
                {
                    DialogResult = true,
                    Language = (C2Language)dialog.LanguageSetting,
                    StartAction = (C2StartAction)dialog.StartActionSetting,
                    HighlightsOutlyingLevelConstants = dialog.HighlightsOutlyingLevelConstants,
                    HighlightsTopSongs = dialog.HighlightsTopSongs,
                    CascadesAchievements = dialog.CascadesAchievements,
                };
            }
            else
            {
                return new SettingsDialogResult { DialogResult = false };
            }
        }

        public SetValueDialogResult ShowSetValueDialog()
        {
            SetValueDialog dialog = new() { Owner = Owner };
            if (dialog.ShowDialog() == true)
            {
                return new SetValueDialogResult
                {
                    DialogResult = true,
                    SetsMM = dialog.SetsMM,
                    SetsTP = dialog.SetsTP,
                    SetsMxm = dialog.SetsMxm,
                    IsMM = dialog.IsMM,
                    TP = dialog.TP,
                    IsMxm = dialog.IsMxm
                };
            }
            else
            {
                return new SetValueDialogResult { DialogResult = false };
            }
        }

        public void ShowStatisticsDialog(IEnumerable songs)
        {
            StatisticsDialog dialog = new([.. songs.Cast<C2SongViewModel>()]) { Owner = Owner };
            dialog.ShowDialog();
        }

        public void ShowAboutDialog()
        {
            AboutDialog dialog = new() { Owner = Owner };
            dialog.ShowDialog();
        }

        #endregion

        // Constants
        const string FILTER = "Cytus II Rating Files (*.c2sr)|*.c2r|JSON Files|*.json|All Files (*.*)|*.*";
        const string DEFAULT_EXT = ".c2sr";
    }
}
