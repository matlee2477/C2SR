using C2SR.Services;
using C2SR.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace C2SR.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string fileName)
        {
            InitializeComponent();

            C2DialogService dialogService = new(this);
            vm = new(dialogService);
            DataContext = vm;
            vm.Initialize(fileName);
            vm.SelectAllExecuted += (s, e) => listView.SelectAll();
            vm.ExitExecuted += (s, e) => Close();

            // Set shortcuts
            {
                KeyGesture newCommandGesture = new(Key.N, ModifierKeys.Control);
                KeyGesture loadCommandGesture = new(Key.O, ModifierKeys.Control);
                KeyGesture saveCommandGesture = new(Key.S, ModifierKeys.Control);
                KeyGesture saveAsCommandGesture = new(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
                KeyGesture undoCommandGesture = new(Key.Z, ModifierKeys.Control);
                KeyGesture redoCommandGesture = new(Key.Y, ModifierKeys.Control);
                KeyGesture cutCommandGesture = new(Key.X, ModifierKeys.Control);
                KeyGesture copyCommandGesture = new(Key.C, ModifierKeys.Control);
                KeyGesture pasteCommandGesture = new(Key.V, ModifierKeys.Control);
                KeyGesture setSelectionCommandGesture = new(Key.F2);
                KeyGesture deleteSelectionCommandGesture = new(Key.Delete);
                KeyGesture selectAllCommandGesture = new(Key.A, ModifierKeys.Control);
                KeyGesture clearCommandGesture = new(Key.Delete, ModifierKeys.Control);
                KeyGesture exitCommandGesture = new(Key.F4, ModifierKeys.Alt);
                InputBindings.Add(new InputBinding(vm.NewDocumentCommand, newCommandGesture));
                InputBindings.Add(new InputBinding(vm.LoadCommand, loadCommandGesture));
                InputBindings.Add(new InputBinding(vm.SaveCommand, saveCommandGesture));
                InputBindings.Add(new InputBinding(vm.SaveAsCommand, saveAsCommandGesture));
                InputBindings.Add(new InputBinding(vm.UndoCommand, undoCommandGesture));
                InputBindings.Add(new InputBinding(vm.RedoCommand, redoCommandGesture));
                InputBindings.Add(new InputBinding(vm.CutCommand, cutCommandGesture));
                InputBindings.Add(new InputBinding(vm.CopyCommand, copyCommandGesture));
                InputBindings.Add(new InputBinding(vm.PasteCommand, pasteCommandGesture));
                InputBindings.Add(new InputBinding(vm.SetSelectionCommand, setSelectionCommandGesture));
                InputBindings.Add(new InputBinding(vm.DeleteSelectionCommand, deleteSelectionCommandGesture));
                InputBindings.Add(new InputBinding(vm.SelectAllCommand, selectAllCommandGesture));
                InputBindings.Add(new InputBinding(vm.ClearCommand, clearCommandGesture));
                InputBindings.Add(new InputBinding(vm.ExitCommand, exitCommandGesture));
            }

            // Load registry
            {
                using C2RegistryService reg = new();
                Left = reg.WindowLeft;
                Top = reg.WindowTop;
                Width = reg.WindowWidth;
                Height = reg.WindowHeight;
                WindowState = reg.IsMaximized ? WindowState.Maximized : WindowState.Normal;
            }
        }

        // Fields
        readonly MainWindowViewModel vm;

        // Event Handlers
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.SelectedSongs = listView.SelectedItems.Cast<C2SongViewModel>();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            vm.QuerySaveChanges(out bool cancel);
            e.Cancel = cancel;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Save registry
            using C2RegistryService reg = new();
            reg.WindowLeft = (int)Left;
            reg.WindowTop = (int)Top;
            reg.WindowWidth = (int)Width;
            reg.WindowHeight = (int)Height;
            reg.IsMaximized = WindowState == WindowState.Maximized;
        }
    }
}
