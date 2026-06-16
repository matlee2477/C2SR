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
            tp = 0;
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

        decimal tp;
        public decimal TPValue => tp;
        public string TP
        {
            get => tp.ToString("N2");
            set
            {
                if (decimal.TryParse(value, out decimal newValue))
                {
                    if (newValue < 0) newValue = 0;
                    if (newValue > 100) newValue = 100;
                    tp = newValue;
                }
                else
                {
                    tp = 0;
                }
                OnPropertyChanged(nameof(TP));

                if (tp == 100)
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

                if (field)
                {
                    SetsMM = true;
                    IsMM = true;
                    SetsTP = true;
                    TP = "100";
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
