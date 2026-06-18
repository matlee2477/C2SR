using C2SR.Services;
using System.ComponentModel;
using System.Windows;

namespace C2SR.Views
{
    /// <summary>
    /// Interaction logic for SetValueDialog.xaml
    /// </summary>
    public partial class SetValueDialog : Window, INotifyPropertyChanged
    {
        public SetValueDialog()
        {
            InitializeComponent();

            DataContext = this;

            SetsMM = false;
            SetsTP = false;
            SetsMxm = false;
            IsMM = false;
            TP = 0;
            IsMxm = false;
        }

        #region Properties
        public bool SetsMM
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(SetsMM));
            }
        }

        public bool SetsTP
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(SetsTP));
            }
        }

        public bool SetsMxm
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(SetsMxm));
            }
        }

        public bool IsMM
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(IsMM));
            }
        }

        public decimal TP
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(TP));

                if (C2SettingService.Instance.CascadesAchievements && field == 100)
                {
                    SetsMM = true;
                    IsMM = true;
                }
            }
        }

        public bool IsMxm
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(IsMxm));

                if (C2SettingService.Instance.CascadesAchievements && field)
                {
                    SetsMM = true;
                    IsMM = true;
                    SetsTP = true;
                    TP = 100;
                }
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
