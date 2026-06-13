using C2SR.Models;
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
            string fileName;
            if (e.Args.Length > 0)
            {
                fileName = e.Args[0];
            }
            else
            {
                fileName = string.Empty;
            }

            C2Registry reg = new();
            try
            {
                // TODO

                MainWindow view = new(fileName);
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
