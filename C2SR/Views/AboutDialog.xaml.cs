using System.IO;
using System.Reflection;
using System.Windows;
using static C2SR.App.Constants;

namespace C2SR.Views
{
    /// <summary>
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : Window
    {
        public AboutDialog()
        {
            InitializeComponent();

            var companyAttribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCompanyAttribute>();
            textBlock_Author.Text = string.Format(textBlock_Author.Text, companyAttribute?.Company);

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            textBlock_Version.Text = string.Format(textBlock_Version.Text, $"{version?.Major}.{version?.Minor}.{version?.Revision}");

            using FileStream fs = new(PATH_LICENSE, FileMode.Open, FileAccess.Read);
            using StreamReader reader = new(fs);
            textBox_License.Text = reader.ReadToEnd();
        }

        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
