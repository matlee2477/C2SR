using C2SR.Resources;
using C2SR.Services;
using C2SR.Services.RegistryServices;
using C2SR.ViewModels;
using C2SR.Views;
using System.Globalization;
using System.Windows;

namespace C2SR
{
    /// <summary>
    /// Interaction logic for xaml
    /// </summary>
    public partial class App : Application
    {
        // Properties
        MainWindow? view;

        #region Event Handlers
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string fileName = (e.Args.Length > 0) ? e.Args[0] : string.Empty;
            RegistryService reg = new();
            try
            {
                // Load settings
                C2SettingService.Instance.Language = (C2Language)reg.GetSetting("Language", 0);
                C2SettingService.Instance.StartAction = (C2StartAction)reg.GetSetting("StartAction", 1);
                C2SettingService.Instance.LastFileName = reg.LastFileName;
                C2SettingService.Instance.HighlightsOutlyingLevelConstants = reg.GetSetting("HighlightsOutlyingLevelConstants", true);
                C2SettingService.Instance.HighlightsBossSongs = reg.GetSetting("HighlightsBossSongs", true);
                C2SettingService.Instance.HighlightsTopSongs = reg.GetSetting("HighlightsTopSongs", true);
                C2SettingService.Instance.CascadesAchievements = reg.GetSetting("CascadesAchievements", true);

                // Set language
                string locale = C2SettingService.Instance.Language switch
                {
                    C2Language.Korean => "ko-KR",
                    C2Language.Japanese => "ja-JP",
                    C2Language.English or _ => "en-US",
                };
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(locale);

                // Define views and view models
                view = new();
                MainWindowViewModel vm = new();
                view.DataContext = vm;

                // Load window state
                view.Left = reg.WindowLeft;
                view.Top = reg.WindowTop;
                view.Width = reg.WindowWidth;
                view.Height = reg.WindowHeight;
                view.WindowState = reg.IsMaximized ? WindowState.Maximized : WindowState.Normal;

                // Add event handlers
                {
                    view.SelectionChanged += (sender, e) => vm.ApplySelection(e.SelectedItems);
                    view.ApplyFiltersExecuted += (sender, e) => vm.ApplyFilters(e.Filter);
                    view.Closing += (sender, e) =>
                    {
                        vm.QuerySaveChanges(out bool cancel);
                        e.Cancel = cancel;
                    };
                    view.Closed += (sender, e) =>
                    {
                        using RegistryService reg = new();
                        reg.LastFileName = vm.FileName;
                        reg.SetVisibility("SearchBar", vm.IsSearchBarVisible);
                        reg.SetVisibility("Filters", vm.IsFiltersVisible);
                        reg.SetVisibility("StatusBar", vm.IsStatusBarVisible);
                    };

                    vm.RefreshListViewRequested += (sender, e) => view.RefreshListView();
                    vm.SelectAllExecuted += (sender, e) => view.SelectAll();
                    vm.ExitExecuted += (sender, e) => view.Close();
                }

                vm.Initialize(fileName);
                view.Show();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.StackTrace);
#endif
                MessageBox.Show(Strings.MessageBox_Error_Startup, Strings.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(-1);
            }
            finally
            {
                reg.Close();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // Save window state
            if (view != null)
            {
                using RegistryService reg = new();
                reg.WindowLeft = (int)view.Left;
                reg.WindowTop = (int)view.Top;
                reg.WindowWidth = (int)view.Width;
                reg.WindowHeight = (int)view.Height;
                reg.IsMaximized = view.WindowState == WindowState.Maximized;
            }
        }

        #endregion

        // Global Constants
        public static class Constants
        {
            public const string PATH_SONGS_JSON = @".\data\songs.json";
            public const string PATH_RANKS_JSON = @".\data\ranks.json";
            public const string PATH_DROPDOWN_JSON = @".\data\dropdownitems.json";
            public const string PATH_CHECKSUM = @".\data\checksum.dat";
            public const string PATH_LICENSE = @".\data\LICENSE";
            public const string FILE_FILTER = "Cytus II Skill Rate Document|*.c2sr|All Files|*.*";
            public const string FILE_DEFAULT_EXT = ".c2sr";
            public const int LEVEL_THRESHOLD = 14;
            public const int TOTAL_SCORE_SONG_COUNT = 30;
            public const decimal SCORE_BONUS_MM = 0.5M;
            public const decimal SCORE_BONUS_TP100 = 0.5M;
        }
    }
}
