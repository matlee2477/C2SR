using C2SR.EventHandling;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;

namespace C2SR.Views
{
    /// <summary>
    /// Interaction logic for SearchBar.xaml
    /// </summary>
    public partial class SearchBar : UserControl
    {
        public SearchBar()
        {
            InitializeComponent();

            SearchPropertyChanged?.Invoke(this, new(SearchOption, SearchTerm, IsCaseSensitive));
        }

        // Fields
        public static readonly DependencyProperty SearchOptionProperty = DependencyProperty.Register(
            nameof(SearchOption),
            typeof(SearchOption),
            typeof(SearchBar),
            new PropertyMetadata(SearchOption.Name));

        public static readonly DependencyProperty SearchTermProperty = DependencyProperty.Register(
            nameof(SearchTerm),
            typeof(string),
            typeof(SearchBar),
            new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsCaseSensitiveProperty = DependencyProperty.Register(
            nameof(IsCaseSensitive),
            typeof(bool),
            typeof(SearchBar),
            new PropertyMetadata(false));

        // Properties
        public SearchOption SearchOption
        {
            get => (SearchOption)GetValue(SearchOptionProperty);
            set => SetValue(SearchOptionProperty, value);
        }

        public string SearchTerm
        {
            get => (string)GetValue(SearchTermProperty);
            set => SetValue(SearchTermProperty, value);
        }

        public bool IsCaseSensitive
        {
            get => (bool)GetValue(IsCaseSensitiveProperty);
            set => SetValue(IsCaseSensitiveProperty, value);
        }

        // Events
        public event SearchBarEventHandler? SearchPropertyChanged;
        public event SearchBarEventHandler? SearchExecuted;

        // Event handlers
        private void button_Search_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSearch();
        }

        #region Commands
        public RelayCommand SearchCommand => new(ExecuteSearch);

        public void ExecuteSearch()
        {
            SearchBarEventArgs e2 = new(SearchOption, SearchTerm, IsCaseSensitive);
            SearchPropertyChanged?.Invoke(this, e2);
            SearchExecuted?.Invoke(this, e2);
        }

        #endregion
    }
}
