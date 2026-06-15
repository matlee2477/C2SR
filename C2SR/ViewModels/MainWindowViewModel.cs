using C2SR.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections;
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
            songs = [];
            clipboard = [];
            this.dialogService = dialogService;
            FileName = string.Empty;
            IsSaved = true;

            NewDocumentCommand = new(ExecuteNewDocument);
            LoadCommand = new(ExecuteLoad);
            SaveCommand = new(ExecuteSave);
            SaveAsCommand = new(ExecuteSaveAs);
            SettingsCommand = new(ExecuteSettings);
            UndoCommand = new(ExecuteUndo, CanUndo);
            RedoCommand = new(ExecuteRedo, CanRedo);
            CutCommand = new(ExecuteCut, CanEdit);
            CopyCommand = new(ExecuteCopy, CanEdit);
            PasteCommand = new(ExecutePaste, CanPaste);
            SetSelectionCommand = new(ExecuteSetSelection, CanEdit);
            DeleteSelectionCommand = new(ExecuteDeleteSelection, CanEdit);
            SelectAllCommand = new(ExecuteSelectAll);
            ClearCommand = new(ExecuteClear);
            ViewStatisticsCommand = new(ExecuteViewStatistics);
            AboutCommand = new(ExecuteAbout);
            ExitCommand = new(ExecuteExit);

            SelectedSongs = [];
        }

        // Fields
        readonly ObservableCollection<C2SongViewModel> songs;
        readonly UndoableCommandStack undoStack = UndoableCommandStack.Instance;
        C2ClipboardField[] clipboard;
        readonly IDialogService dialogService;

        #region Properties
        public IEnumerable<C2SongViewModel> Songs => songs;

        public string FileName
        {
            get;
            set
            {
                field = value;
                ChangeTitleRequested?.Invoke(this, new(FileName, IsSaved));
            }
        }

        public bool IsSaved
        {
            get;
            set
            {
                field = value;
                ChangeTitleRequested?.Invoke(this, new(FileName, IsSaved));
            }
        }

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
        public event ChangeTitleRequestedEventHandler? ChangeTitleRequested;
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
                    foreach (JsonNode node in arr.OfType<JsonObject>())
                    {
                        if (node is not JsonObject obj) continue;

                        long id = BitConverter.ToInt64(Convert.FromHexString(obj["ID"]!.GetValue<string>()));
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
                    ExecuteExit();
                }
            }

            // Load document
            {
                if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                {
                    Load(fileName);
                }
                else
                {
                    // TODO
                }
            }
        }

        void NewDocument()
        {
            foreach (var song in Songs)
            {
                song.SetMM(false, C2SongSetPropertyOption.Silent);
                song.SetTP(0, C2SongSetPropertyOption.Silent);
                song.SetMxm(false, C2SongSetPropertyOption.Silent);
            }

            FileName = string.Empty;
            IsSaved = true;
            undoStack.Clear();
        }

        void Load(string fileName)
        {
            using FileStream fs = new(fileName, FileMode.Open, FileAccess.Read);
            using StreamReader reader = new(fs);
            string code = reader.ReadToEnd();

            JsonArray arr = JsonNode.Parse(code)!.AsArray();
            foreach (JsonObject obj in arr.OfType<JsonObject>())
            {
                long id = BitConverter.ToInt64(Convert.FromHexString(obj["ID"]!.GetValue<string>()));
                C2SongViewModel? song = songs.FirstOrDefault(s => s.Song.ID == id);
                if (song != null)
                {
                    bool isMM = obj["MM"]!.GetValue<bool>();
                    decimal tp = obj["TP"]!.GetValue<decimal>();
                    bool isMxm = obj["MxM"]!.GetValue<bool>();
                    song.SetMM(isMM, C2SongSetPropertyOption.Silent);
                    song.SetTP(tp, C2SongSetPropertyOption.Silent);
                    song.SetMxm(isMxm, C2SongSetPropertyOption.Silent);
                }
            }

            FileName = fileName;
            IsSaved = true;
            undoStack.Clear();
        }

        void Save(string fileName)
        {
            JsonArray arr = [];
            foreach (var song in songs)
            {
                JsonObject obj = new()
                {
                    ["ID"] = Convert.ToHexString(BitConverter.GetBytes(song.Song.ID)),
                    ["MM"] = song.IsMM,
                    ["TP"] = song.Song.TP,
                    ["MxM"] = song.IsMxm
                };
                arr.Add(obj);
            }

            using FileStream fs = new(fileName, FileMode.Create, FileAccess.Write);
            using StreamWriter writer = new(fs);
            writer.Write(arr.ToJsonString());

            FileName = fileName;
            IsSaved = true;
        }

        public void QuerySaveChanges(out bool cancel)
        {
            if (!IsSaved)
            {
                switch (dialogService.QuerySaveChangesDialog())
                {
                    case MessageBoxResult.Yes:
                        ExecuteSave();
                        cancel = !IsSaved;
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
                IsSaved = false;
            }
        }

        public void ApplySelection(IList selectedItems)
        {
            SelectedSongs = selectedItems.Cast<C2SongViewModel>();
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
            IsSaved = false;
        }

        private void C2SongViewModel_TPChanged(object sender, C2TPChangedEventArgs e)
        {
            IsSaved = false;
        }

        private void C2SongViewModel_MxmChanged(object sender, C2MxmChangedEventArgs e)
        {
            IsSaved = false;
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

        void ExecuteNewDocument()
        {
            QuerySaveChanges(out bool cancel);
            if (cancel) return;

            NewDocument();
        }

        void ExecuteLoad()
        {
            QuerySaveChanges(out bool cancel);
            if (cancel) return;

            if (dialogService.ShowOpenFileDialog(out string fileName) == true)
            {
                try
                {
                    Load(fileName);
                }
                catch
                {
                    dialogService.ShowOpenErrorDialog();
                    NewDocument();
                }
            }
        }

        void ExecuteSave()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                ExecuteSaveAs();
            }
            else
            {
                try
                {
                    Save(FileName);
                }
                catch
                {
                    dialogService.ShowSaveErrorDialog();
                }
            }
        }

        void ExecuteSaveAs()
        {
            if (dialogService.ShowSaveFileDialog(out string fileName) == true)
            {
                try
                {
                    Save(fileName);
                }
                catch
                {
                    dialogService.ShowSaveErrorDialog();
                }
            }
        }

        void ExecuteSettings()
        {
            MessageBox.Show("Settings command executed");
        }

        void ExecuteUndo()
        {
            undoStack.Undo();
            UndoCommand.NotifyCanExecuteChanged();
            RedoCommand.NotifyCanExecuteChanged();
        }

        void ExecuteRedo()
        {
            undoStack.Redo();
            UndoCommand.NotifyCanExecuteChanged();
            RedoCommand.NotifyCanExecuteChanged();
        }

        void ExecuteCut()
        {
            ExecuteCopy();
            ExecuteDeleteSelection();
        }

        void ExecuteCopy()
        {
            clipboard = [.. SelectedSongs.Select(song => new C2ClipboardField() { IsMM = song.IsMM, TP = song.Song.TP, IsMxm = song.IsMxm })];
            PasteCommand.NotifyCanExecuteChanged();
        }

        void ExecutePaste()
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
                IsSaved = false;
            }
        }

        void ExecuteSetSelection()
        {
            SetValueDialogResult result = dialogService.ShowSetValueDialog();
            if (result.DialogResult)
            {
                SetValue(SelectedSongs,
                    result.SetsMM ? result.IsMM : null,
                    result.SetsTP ? result.TP : null,
                    result.SetsMxm ? result.IsMxm : null);
            }
        }

        void ExecuteDeleteSelection()
        {
            SetValue(SelectedSongs, isMM: false, tp: 0, isMxm: false);
        }

        void ExecuteSelectAll()
        {
            SelectAllExecuted?.Invoke(this, EventArgs.Empty);
        }

        void ExecuteClear()
        {
            SetValue(Songs, isMM: false, tp: 0, isMxm: false);
        }

        void ExecuteViewStatistics()
        {
            
        }

        void ExecuteAbout()
        {
            dialogService.ShowAboutDialog();
        }

        void ExecuteExit()
        {
            ExitExecuted?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
