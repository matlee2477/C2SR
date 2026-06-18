using C2SR.Resources;
using C2SR.Services;
using System.ComponentModel;
using System.Windows;

namespace C2SR.Views
{
    /// <summary>
    /// Interaction logic for SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window, INotifyPropertyChanged
    {
        public SettingsDialog()
        {
            InitializeComponent();

            DataContext = this;
            isFirstLanguageChange = false;

            LanguageSetting = (int)C2SettingService.Instance.Language;
            StartActionSetting = (int)C2SettingService.Instance.StartAction;
            HighlightsOutlyingLevelConstants = C2SettingService.Instance.HighlightsOutlyingLevelConstants;
            HighlightsTopSongs = C2SettingService.Instance.HighlightsTopSongs;
            CascadesAchievements = C2SettingService.Instance.CascadesAchievements;

            isFirstLanguageChange = true;
        }

        // Fields
        bool isFirstLanguageChange;

        #region Properties
        public int LanguageSetting
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(LanguageSetting));

                if (isFirstLanguageChange)
                {
                    MessageBox.Show(Strings.MessageBox_LanguageChanged, Strings.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    isFirstLanguageChange = false;
                }
            }
        }

        public int StartActionSetting
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(StartActionSetting));
            }
        }

        public bool HighlightsOutlyingLevelConstants
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(HighlightsOutlyingLevelConstants));
            }
        }

        public bool HighlightsTopSongs
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(HighlightsTopSongs));
            }
        }

        public bool CascadesAchievements
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(CascadesAchievements));
            }
        }

        #endregion

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Event Handlers
        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        #endregion
    }
}
