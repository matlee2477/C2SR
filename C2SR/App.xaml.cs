using C2SR.Models;
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

            try
            {
                var reg = RegistryLoader.Instance;

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
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // Dispose singleton instances at the end of application
            RegistryLoader.Instance.Dispose();
        }
    }
}
