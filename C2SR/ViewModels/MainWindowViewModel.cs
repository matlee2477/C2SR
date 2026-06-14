using C2SR.Models;
using C2SR.Services;
using C2SR.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json.Nodes;
using System.Windows;

namespace C2SR.ViewModels
{
    class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel(IDialogService dialogService)
        {
            doc = C2Document.Instance;
            songs = [];
            clipboard = [];
            this.dialogService = dialogService;

            NewDocumentCommand = new(NewDocument);
            LoadCommand = new(Load);
            SaveCommand = new(Save);
            SaveAsCommand = new(SaveAs);
            SettingsCommand = new(Settings);
            UndoCommand = new(Undo, CanUndo);
            RedoCommand = new(Redo, CanRedo);
            CutCommand = new(Cut, CanEdit);
            CopyCommand = new(Copy, CanEdit);
            PasteCommand = new(Paste, CanPaste);
            SetSelectionCommand = new(SetSelection, CanEdit);
            DeleteSelectionCommand = new(DeleteSelection, CanEdit);
            SelectAllCommand = new(SelectAll);
            ClearCommand = new(Clear);
            ViewStatisticsCommand = new(ViewStatistics);
            AboutCommand = new(About);
            ExitCommand = new(Exit);

            SelectedSongs = [];
        }

        // Fields
        readonly C2Document doc;
        readonly ObservableCollection<C2SongViewModel> songs;
        readonly UndoableCommandStack undoStack = UndoableCommandStack.Instance;
        C2ClipboardField[] clipboard;
        readonly IDialogService dialogService;

        #region Properties
        public IEnumerable<C2SongViewModel> Songs => songs;

        public IEnumerable<C2SongViewModel> SelectedSongs
        {
            get;
            set
            {
                field = value;
                CutCommand.NotifyCanExecuteChanged();
                CopyCommand.NotifyCanExecuteChanged();
                PasteCommand.NotifyCanExecuteChanged();
                SetSelectionCommand.NotifyCanExecuteChanged();
                DeleteSelectionCommand.NotifyCanExecuteChanged();
            }
        }

        public bool IsFiltersVisible
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(IsFiltersVisible));
                OnPropertyChanged(nameof(FiltersVisibility));
            }
        }

        public bool IsSearchBarVisible
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(IsSearchBarVisible));
                OnPropertyChanged(nameof(SearchBarVisibility));
            }
        }

        public bool IsStatusBarVisible
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(IsStatusBarVisible));
                OnPropertyChanged(nameof(StatusBarVisibility));
            }
        }

        public Visibility FiltersVisibility => IsFiltersVisible ? Visibility.Visible : Visibility.Collapsed;
        public Visibility SearchBarVisibility => IsSearchBarVisible ? Visibility.Visible : Visibility.Collapsed;
        public Visibility StatusBarVisibility => IsStatusBarVisible ? Visibility.Visible : Visibility.Collapsed;

        #endregion

        // Events
        public event EventHandler? SelectAllExecuted;
        public event EventHandler? ExitExecuted;

        #region Methods
        public void Initialize(string fileName)
        {
            // Load song data
            {
                try
                {
                    using FileStream fs = new(".\\data\\songs.json", FileMode.Open, FileAccess.Read);
                    using StreamReader reader = new(fs);
                    string code = reader.ReadToEnd();

                    JsonArray arr = JsonNode.Parse(code)!.AsArray();
                    foreach (JsonObject obj in arr.OfType<JsonObject>())
                    {
                        if (obj.ContainsKey("comment")) continue;

                        long id = obj["ID"]!.GetValue<long>();
                        string name = obj["name"]!.GetValue<string>();
                        string artist = obj["artist"]!.GetValue<string>();
                        decimal bpm = obj["BPM"]!.GetValue<decimal>();
                        string version = obj["version"]!.GetValue<string>();
                        string chapter = obj["chapter"]!.GetValue<string>();
                        string chartType = obj["chart"]!.GetValue<string>();
                        decimal level = obj["level"]!.GetValue<decimal>();
                        decimal levelConstant = obj["const"]!.GetValue<decimal>();

                        C2SongViewModel song = new(new(id, name, artist, bpm, version, chapter, chartType, level, levelConstant));
                        song.MMChanging += C2SongViewModel_MMChanging;
                        song.TPChanging += C2SongViewModel_TPChanging;
                        song.MxmChanging += C2SongViewModel_MxmChanging;
                        song.MMChanged += C2SongViewModel_PropertyChanged;
                        song.TPChanged += C2SongViewModel_TPChanged;
                        song.MxmChanged += C2SongViewModel_MxmChanged;
                        songs.Add(song);
                    }
                }
                catch
                {
                    MessageBox.Show("An error occurred while loading the game data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // Load document
            {
                if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                {
                    doc.Load(fileName);
                }
                else
                {

                }
            }
        }

        public void QuerySaveChanges(out bool cancel)
        {
            if (!doc.IsSaved)
            {
                switch (dialogService.QuerySaveChangesDialog())
                {
                    case MessageBoxResult.Yes:
                        Save();
                        cancel = !doc.IsSaved;
                        break;
                    case MessageBoxResult.No:
                        cancel = false;
                        break;
                    default:
                        cancel = true;
                        break;
                }
            }
            else
            {
                cancel = false;
            }
        }

        void SetValue(IEnumerable<C2SongViewModel> songs, bool? isMM = null, decimal? tp = null, bool? isMxm = null)
        {
            UndoableCommandCollection commands = [];
            foreach (var song in songs)
            {
                if (isMM.HasValue)
                {
                    var oldMM = song.IsMM;
                    if (oldMM != isMM.Value)
                    {
                        song.SetMM(isMM.Value, C2SongSetPropertyOption.Silent);
                        commands.Add(new C2MMUndoableCommand(song, oldMM, isMM.Value));
                    }
                }
                if (tp.HasValue)
                {
                    var oldTP = song.Song.TP;
                    if (oldTP != tp.Value)
                    {
                        song.SetTP(tp.Value, C2SongSetPropertyOption.Silent);
                        commands.Add(new C2TPUndoableCommand(song, oldTP, tp.Value));
                    }
                }
                if (isMxm.HasValue)
                {
                    var oldMxm = song.IsMxm;
                    if (oldMxm != isMxm.Value)
                    {
                        song.SetMxm(isMxm.Value, C2SongSetPropertyOption.Silent);
                        commands.Add(new C2MxmUndoableCommand(song, oldMxm, isMxm.Value));
                    }
                }
            }

            // If no changes were made, do not add to undo stack
            // This will optimize the undo stack by preventing redundant entries
            if (commands.Count > 0)
            {
                undoStack.AddUndoCommand(commands);
                UndoCommand.NotifyCanExecuteChanged();
                RedoCommand.NotifyCanExecuteChanged();
                doc.IsSaved = false;
            }
        }

        #endregion

        #region Event Handlers
        private void C2SongViewModel_MMChanging(object sender, C2MMChangingEventArgs e)
        {
            C2MMUndoableCommand command = new((C2SongViewModel)sender, e.OldValue, e.NewValue);
            undoStack.AddUndoCommand(command);
            UndoCommand.NotifyCanExecuteChanged();
            RedoCommand.NotifyCanExecuteChanged();
        }

        private void C2SongViewModel_TPChanging(object sender, C2TPChangingEventArgs e)
        {
            C2TPUndoableCommand command = new((C2SongViewModel)sender, e.OldValue, e.NewValue);
            undoStack.AddUndoCommand(command);
            UndoCommand.NotifyCanExecuteChanged();
            RedoCommand.NotifyCanExecuteChanged();
        }

        private void C2SongViewModel_MxmChanging(object sender, C2MxmChangingEventArgs e)
        {
            C2MxmUndoableCommand command = new((C2SongViewModel)sender, e.OldValue, e.NewValue);
            undoStack.AddUndoCommand(command);
            UndoCommand.NotifyCanExecuteChanged();
            RedoCommand.NotifyCanExecuteChanged();
        }

        private void C2SongViewModel_PropertyChanged(object sender, C2MMChangedEventArgs e)
        {
            doc.IsSaved = false;
        }

        private void C2SongViewModel_TPChanged(object sender, C2TPChangedEventArgs e)
        {
            doc.IsSaved = false;
        }

        private void C2SongViewModel_MxmChanged(object sender, C2MxmChangedEventArgs e)
        {
            doc.IsSaved = false;
        }

        #endregion

        #region Commands
        public RelayCommand NewDocumentCommand { get; }
        public RelayCommand LoadCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand SaveAsCommand { get; }
        public RelayCommand SettingsCommand { get; }
        public RelayCommand UndoCommand { get; }
        public RelayCommand RedoCommand { get; }
        public RelayCommand CutCommand { get; }
        public RelayCommand CopyCommand { get; }
        public RelayCommand PasteCommand { get; }
        public RelayCommand SetSelectionCommand { get; }
        public RelayCommand DeleteSelectionCommand { get; }
        public RelayCommand SelectAllCommand { get; }
        public RelayCommand ClearCommand { get; }
        public RelayCommand ViewStatisticsCommand { get; }
        public RelayCommand AboutCommand { get; }
        public RelayCommand ExitCommand { get; }

        bool CanEdit() => SelectedSongs.Any();
        bool CanUndo() => undoStack.CanUndo;
        bool CanRedo() => undoStack.CanRedo;
        bool CanPaste() => CanEdit() && clipboard.Length > 0;

        void NewDocument()
        {
            QuerySaveChanges(out bool cancel);
            if (!cancel)
            {
                doc.FileName = string.Empty;
                doc.IsSaved = false;
                foreach (var song in Songs)
                {
                    song.SetMM(false, C2SongSetPropertyOption.Silent);
                    song.SetTP(0, C2SongSetPropertyOption.Silent);
                    song.SetMxm(false, C2SongSetPropertyOption.Silent);
                }
            }
        }

        void Load()
        {
            QuerySaveChanges(out bool cancel);
            if (cancel) return;

            if (dialogService.ShowOpenFileDialog(out string fileName) == true)
            {
                try
                {
                    var fileData = doc.Load(fileName);
                    foreach (var data in fileData)
                    {
                        var song = Songs.FirstOrDefault(s => s.Song.ID == data.ID);
                        if (song != null)
                        {
                            song.SetMM(data.IsMM, C2SongSetPropertyOption.Silent);
                            song.SetTP(data.TP, C2SongSetPropertyOption.Silent);
                            song.SetMxm(data.IsMxm, C2SongSetPropertyOption.Silent);
                        }
                    }
                }
                catch
                {
                    dialogService.ShowOpenErrorDialog();
                }
            }
        }

        void Save()
        {
            if (string.IsNullOrEmpty(doc.FileName))
            {
                SaveAs();
            }
            else
            {
                try
                {
                    doc.Save(doc.FileName, [.. Songs.Select(s => s.Song)]);
                }
                catch
                {
                    dialogService.ShowSaveErrorDialog();
                }
            }
        }

        void SaveAs()
        {
            if (dialogService.ShowSaveFileDialog(out string fileName) == true)
            {
                try
                {
                    doc.Save(fileName, [.. Songs.Select(s => s.Song)]);
                }
                catch
                {
                    dialogService.ShowSaveErrorDialog();
                }
            }
        }

        void Settings()
        {
            MessageBox.Show("Settings command executed");
        }

        void Undo()
        {
            undoStack.Undo();
            UndoCommand.NotifyCanExecuteChanged();
            RedoCommand.NotifyCanExecuteChanged();
        }

        void Redo()
        {
            undoStack.Redo();
            UndoCommand.NotifyCanExecuteChanged();
            RedoCommand.NotifyCanExecuteChanged();
        }

        void Cut()
        {
            Copy();
            DeleteSelection();
        }

        void Copy()
        {
            clipboard = [.. SelectedSongs.Select(song => new C2ClipboardField() { IsMM = song.IsMM, TP = song.Song.TP, IsMxm = song.IsMxm })];
            PasteCommand.NotifyCanExecuteChanged();
        }

        void Paste()
        {
            int index = 0;
            UndoableCommandCollection commands = [];
            foreach (var song in SelectedSongs)
            {
                var oldMM = song.IsMM;
                var oldTP = song.Song.TP;
                var oldMxm = song.IsMxm;

                var newMM = clipboard[index % clipboard.Length].IsMM;
                var newTP = clipboard[index % clipboard.Length].TP;
                var newMxm = clipboard[index % clipboard.Length].IsMxm;

                if (oldMM != newMM)
                {
                    song.SetMM(newMM, C2SongSetPropertyOption.Silent);
                    commands.Add(new C2MMUndoableCommand(song, oldMM, newMM));
                }
                if (oldTP != newTP)
                {
                    song.SetTP(newTP, C2SongSetPropertyOption.Silent);
                    commands.Add(new C2TPUndoableCommand(song, oldTP, newTP));
                }
                if (oldMxm != newMxm)
                {
                    song.SetMxm(newMxm, C2SongSetPropertyOption.Silent);
                    commands.Add(new C2MxmUndoableCommand(song, oldMxm, newMxm));
                }

                index++;
            }

            // If no changes were made, do not add to undo stack
            // This will optimize the undo stack by preventing redundant entries
            if (commands.Count > 0)
            {
                undoStack.AddUndoCommand(commands);
                UndoCommand.NotifyCanExecuteChanged();
                RedoCommand.NotifyCanExecuteChanged();
                doc.IsSaved = false;
            }
        }

        void SetSelection()
        {
            SetValueDialog dialog = new();
            if (dialog.ShowDialog() == true)
            {
                SetValue(SelectedSongs,
                    dialog.SetsMM ? dialog.IsMM : null,
                    dialog.SetsTP ? dialog.TPValue : null,
                    dialog.SetsMxm ? dialog.IsMxm : null);
            }
        }

        void DeleteSelection()
        {
            SetValue(SelectedSongs, isMM: false, tp: 0, isMxm: false);
        }

        void SelectAll()
        {
            SelectAllExecuted?.Invoke(this, EventArgs.Empty);
        }

        void Clear()
        {
            SetValue(Songs, isMM: false, tp: 0, isMxm: false);
        }

        void ViewStatistics()
        {

        }

        void About()
        {

        }

        void Exit()
        {
            ExitExecuted?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
