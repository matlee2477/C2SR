using C2SR.Services;
using C2SR.ViewModels;
using C2SR.Views;
using System.Windows;

namespace C2SR
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string fileName = (e.Args.Length > 0) ? e.Args[0] : string.Empty;
            C2RegistryService reg = new();
            try
            {
                // TODO


                MainWindow view = new();
                C2DialogService dialogService = new(view);
                MainWindowViewModel vm = new(dialogService);
                view.DataContext = vm;
                vm.Initialize(fileName);
                vm.ChangeTitleRequested += (sender, e) => view.HandleChangeTitleRequest(e.FileName, e.IsSaved);
                vm.SelectAllExecuted += (sender, e) => view.SelectAll();
                vm.ExitExecuted += (sender, e) => view.Close();
                view.SelectionChanged += (sender, e) => vm.ApplySelection(e.SelectedItems);
                view.Closing += (sender, e) =>
                {
                    vm.QuerySaveChanges(out bool cancel);
                    e.Cancel = cancel;
                };

                view.Show();
            }
            catch
            {
                MessageBox.Show("An error occurred while starting the application. Some essential data files may be removed or damaged.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(-1);
                return;
            }
            finally
            {
                reg.Dispose();
            }
        }
    }
}
