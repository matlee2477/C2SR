using System.IO;
using System.Reflection;
using System.Windows;

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
            textBlock_Author.Text = textBlock_Author.Text.Replace("[0]", companyAttribute?.Company);

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            textBlock_Version.Text = textBlock_Version.Text.Replace("[0]", $"{version?.Major}.{version?.Minor}.{version?.Revision}");

            using FileStream fs = new(".\\data\\LICENSE", FileMode.Open, FileAccess.Read);
            using StreamReader reader = new(fs);
            textBox_License.Text = reader.ReadToEnd();
        }

        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
