using C2SR.Resources;
using C2SR.Services;
using C2SR.Services.DialogServices;
using C2SR.Services.RegistryServices;
using C2SR.ViewModels;
using C2SR.Views;
using System.Globalization;
using System.Windows;

namespace C2SR
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Fields
        MainWindow? view;

        #region Event Handlers
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string fileName = (e.Args.Length > 0) ? e.Args[0] : string.Empty;
            C2RegistryService reg = new();
            try
            {
                // Load settings
                C2SettingService.Instance.Language = (C2Language)reg.GetSetting("Language", 0);
                C2SettingService.Instance.StartAction = (C2StartAction)reg.GetSetting("StartAction", 1);
                C2SettingService.Instance.LastFileName = reg.LastFileName;

                // Set language
                string locale = C2SettingService.Instance.Language switch
                {
                    C2Language.English => "en-US",
                    C2Language.Korean => "ko-KR",
                    C2Language.Japanese => "ja-JP",
                    _ => "en-US",
                };
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(locale);

                // Define services and view models
                view = new();
                C2DialogService dialogService = new(view);
                MainWindowViewModel vm = new(dialogService);
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
                    view.Closing += (sender, e) =>
                    {
                        vm.QuerySaveChanges(out bool cancel);
                        e.Cancel = cancel;
                    };
                    view.Closed += (sender, e) =>
                    {
                        using C2RegistryService reg = new();
                        reg.LastFileName = vm.FileName;
                    };

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
                using C2RegistryService reg = new();
                reg.WindowLeft = (int)view.Left;
                reg.WindowTop = (int)view.Top;
                reg.WindowWidth = (int)view.Width;
                reg.WindowHeight = (int)view.Height;
                reg.IsMaximized = view.WindowState == WindowState.Maximized;
            }
        }

        #endregion
    }
}
