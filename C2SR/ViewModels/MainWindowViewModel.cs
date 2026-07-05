using C2SR.EventHandling;
using C2SR.Resources;
using C2SR.Services;
using C2SR.Services.ChecksumServices;
using C2SR.Services.DialogServices;
using C2SR.Services.JsonServices;
using C2SR.Services.RegistryServices;
using C2SR.Services.UndoServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Media;

namespace C2SR.ViewModels
{
    class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel(IDialogService dialogService)
        {
            songs = [];
            clipboard = [];
            this.dialogService = dialogService;
            StatusBarText = string.Empty;
            FileName = string.Empty;
            IsSaved = true;
            StatusBarText = string.Empty;
            FilteredSongs = [];

            {
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
            }

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
                OnPropertyChanged(nameof(FileName));
            }
        }

        public bool IsSaved
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(IsSaved));
            }
        }

        public string StatusBarText
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(StatusBarText));
            }
        }

        public C2TopSongResult TopSongResult
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(TopSongResult));
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

        public bool IsSearchBarVisible
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(IsSearchBarVisible));
            }
        }

        public bool IsFiltersVisible
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(IsFiltersVisible));
            }
        }

        public bool IsStatusBarVisible
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(IsStatusBarVisible));
            }
        }

        public IEnumerable<C2SongViewModel> FilteredSongs
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(FilteredSongs));
            }
        }

        #endregion

        // Events
        public event ChangeTitleRequestedEventHandler? ChangeTitleRequested;
        public event EventHandler? RefreshListViewRequested;
        public event EventHandler? SelectAllExecuted;
        public event EventHandler? ExitExecuted;

        #region Methods
        public void Initialize(string fileName)
        {
            // If debug mode, create checksum; otherwise, verify checksum
            {
                C2ChecksumService checksumService = new();
                checksumService.TargetFiles = [PATH_SONGS_JSON, PATH_RANKS_JSON, PATH_DROPDOWN_JSON];
#if DEBUG
                checksumService.CreateChecksum(PATH_CHECKSUM);
#else
                Exceptions.ChecksumMismatchException.ThrowIfChecksumMismatch(() => checksumService.VerifyChecksum(PATH_CHECKSUM));
#endif
            }

            // Load song data
            {
                C2JsonService jsonService = new();
                string code = jsonService.LoadJson(PATH_SONGS_JSON);

                JsonArray arr = JsonNode.Parse(code)!.AsArray();
                foreach (JsonNode node in arr.OfType<JsonNode>())
                {
                    if (node is not JsonObject obj) continue;

                    long id = BitConverter.ToInt64(Convert.FromHexString(obj["ID"]!.GetValue<string>()));
                    string name = obj["name"]?.GetValue<string>() ?? string.Empty;
                    string artist = obj["artist"]?.GetValue<string>() ?? string.Empty;
                    decimal bpm = obj["BPM"]?.GetValue<decimal>() ?? 0;
                    string version = obj["version"]?.GetValue<string>() ?? string.Empty;
                    string chapter = obj["chapter"]?.GetValue<string>() ?? string.Empty;
                    string chartType = obj["chart"]?.GetValue<string>() ?? string.Empty;
                    decimal level = obj["level"]?.GetValue<decimal>() ?? 12;
                    decimal levelConstant = obj["const"]?.GetValue<decimal>() ?? level;

                    C2SongViewModel song = new(new(id, name, artist, bpm, version, chapter, chartType, level, levelConstant));
                    song.MMChanging += C2SongViewModel_MMChanging;
                    song.TPChanging += C2SongViewModel_TPChanging;
                    song.MxmChanging += C2SongViewModel_MxmChanging;
                    song.MMChanged += C2SongViewModel_MMChanged;
                    song.TPChanged += C2SongViewModel_TPChanged;
                    song.MxmChanged += C2SongViewModel_MxmChanged;

                    if (song.Level >= TARGET_LEVEL) songs.Add(song);
                }

                FilteredSongs = songs;
            }

            // Load total score rank criteria
            {
                C2JsonService jsonService = new();
                string code = jsonService.LoadJson(PATH_RANKS_JSON);

                JsonArray arr = JsonNode.Parse(code)!.AsArray();
                foreach (JsonNode node in arr.OfType<JsonNode>())
                {
                    if (node is not JsonObject obj) continue;

                    if (obj.ContainsKey("top"))
                    {
                        // Top criterion
                        string name = obj["top"]!.GetValue<string>();
                        byte r = obj["r"]?.GetValue<byte>() ?? 0;
                        byte g = obj["g"]?.GetValue<byte>() ?? 0;
                        byte b = obj["b"]?.GetValue<byte>() ?? 0;
                        Color color = new() { A = 255, R = r, G = g, B = b };

                        // Calculate top score
                        var topSongs = Songs.OrderByDescending(s => s.LevelConstant).Take(C2TotalScoreService.TOTAL_SCORE_SONG_COUNT);
                        var topSongsWithTP100 = topSongs.Select(s =>
                        {
                            C2SongViewModel newSong = new(new(s.ID, s.Name, s.Artist, s.Bpm, s.Version, s.Chapter, s.ChartType, s.Level, s.LevelConstant));
                            newSong.IsMM = true;
                            newSong.TP = 100;
                            return newSong;
                        });
                        var result = C2TotalScoreService.GetTopSongs(topSongsWithTP100);

                        C2TotalScoreService.Instance.AddRank(name, result.TotalScore, color);
                    }
                    else
                    {
                        // Normal criterion
                        string name = obj[$"{Thread.CurrentThread.CurrentUICulture.Name}"]?.GetValue<string>() ?? string.Empty;
                        decimal score = obj["score"]?.GetValue<decimal>() ?? 100;
                        byte r = obj["r"]?.GetValue<byte>() ?? 0;
                        byte g = obj["g"]?.GetValue<byte>() ?? 0;
                        byte b = obj["b"]?.GetValue<byte>() ?? 0;
                        Color color = Color.FromRgb(r, g, b);

                        C2TotalScoreService.Instance.AddRank(name, score, color);
                    }
                }
            }

            // Load document if necessary
            {
                if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                {
                    Load(fileName);
                }
                else if (C2SettingService.Instance.StartAction == C2StartAction.OpenLastDocument)
                {
                    string lastFileName = C2SettingService.Instance.LastFileName;
                    if (!string.IsNullOrEmpty(lastFileName) && File.Exists(lastFileName))
                    {
                        Load(lastFileName);
                    }
                }
                else
                {
                    NewDocument();
                }
            }

            // Load registry
            using C2RegistryService reg = new();
            IsSearchBarVisible = reg.GetVisibility("SearchBar", true);
            IsFiltersVisible = reg.GetVisibility("Filters", true);
            IsStatusBarVisible = reg.GetVisibility("StatusBar", true);

            ChangeTitleRequested?.Invoke(this, new(FileName, IsSaved));
        }

        void NewDocument()
        {
            foreach (var song in Songs)
            {
                song.SetMM(false, SetPropertyOption.Silent);
                song.SetTP(0, SetPropertyOption.Silent);
                song.SetMxm(false, SetPropertyOption.Silent);
            }

            FileName = string.Empty;
            IsSaved = true;
            StatusBarText = Strings.MainWindow_StatusBarText_NewDocument;
            UpdateTotalScore();
            undoStack.Clear();
        }

        void Load(string fileName)
        {
            try
            {
                C2JsonService jsonService = new();
                string code = jsonService.LoadJson(fileName);

                JsonArray arr = JsonNode.Parse(code)!.AsArray();
                foreach (JsonObject obj in arr.OfType<JsonObject>())
                {
                    long id = BitConverter.ToInt64(Convert.FromHexString(obj["ID"]!.GetValue<string>()));
                    C2SongViewModel? song = songs.FirstOrDefault(s => s.ID == id);
                    if (song != null)
                    {
                        bool isMM = obj["MM"]?.GetValue<bool>() ?? false;
                        decimal tp = obj["TP"]?.GetValue<decimal>() ?? 0;
                        bool isMxm = obj["MxM"]?.GetValue<bool>() ?? false;
                        song.SetMM(isMM, SetPropertyOption.Silent);
                        song.SetTP(tp, SetPropertyOption.Silent);
                        song.SetMxm(isMxm, SetPropertyOption.Silent);
                    }
                }

                FileName = fileName;
                IsSaved = true;
                StatusBarText = string.Format(Strings.MainWindow_StatusBarText_LoadSuccess, fileName);
                UpdateTotalScore();
                undoStack.Clear();
            }
            catch
            {
                dialogService.ShowOpenErrorDialog();
                NewDocument();
                StatusBarText = string.Format(Strings.MainWindow_StatusBarText_LoadFailure, fileName);
            }
        }

        void Save(string fileName)
        {
            try
            {
                JsonArray arr = [];
                foreach (var song in songs)
                {
                    JsonObject obj = new()
                    {
                        ["ID"] = Convert.ToHexString(BitConverter.GetBytes(song.ID)),
                        ["MM"] = song.IsMM,
                        ["TP"] = song.TP,
                        ["MxM"] = song.IsMxm
                    };
                    arr.Add(obj);
                }

                C2JsonService jsonService = new();
                jsonService.SaveJson(fileName, arr.ToJsonString());

                FileName = fileName;
                IsSaved = true;
                StatusBarText = string.Format(Strings.MainWindow_StatusBarText_SaveSuccess, fileName);
            }
            catch
            {
                dialogService.ShowSaveErrorDialog();
                StatusBarText = string.Format(Strings.MainWindow_StatusBarText_SaveFailure, fileName);
            }
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
                        song.SetMM(isMM.Value, SetPropertyOption.Silent);
                        commands.Add(new C2MMUndoableCommand(song, oldMM, isMM.Value));
                    }
                }
                if (tp.HasValue)
                {
                    var oldTP = song.TP;
                    if (oldTP != tp.Value)
                    {
                        song.SetTP(tp.Value, SetPropertyOption.Silent);
                        commands.Add(new C2TPUndoableCommand(song, oldTP, tp.Value));
                    }
                }
                if (isMxm.HasValue)
                {
                    var oldMxm = song.IsMxm;
                    if (oldMxm != isMxm.Value)
                    {
                        song.SetMxm(isMxm.Value, SetPropertyOption.Silent);
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
                UpdateTotalScore();
            }
        }

        public void ApplySelection(IList selectedItems)
        {
            SelectedSongs = selectedItems.Cast<C2SongViewModel>();
        }

        public void UpdateTotalScore()
        {
            var result = C2TotalScoreService.GetTopSongs(Songs);
            TopSongResult = result;

            foreach (var song in Songs)
            {
                song.IsTopSong = false;
            }
            foreach (var song in TopSongResult.TopSongs)
            {
                song.IsTopSong = true;
            }
        }

        public void ApplyFilters(C2Filter filter)
        {
            IEnumerable<C2SongViewModel> filteredSongs = songs;

            // Apply search
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                filteredSongs = filteredSongs.Where(song =>
                {
                    string propertyValue = filter.SearchOption switch
                    {
                        EventHandling.SearchOption.Name => song.Name,
                        EventHandling.SearchOption.Artist => song.Artist,
                        _ => string.Empty
                    };
                    if (filter.IsCaseSensitive)
                    {
                        return propertyValue.Contains(filter.SearchTerm);
                    }
                    else
                    {
                        return propertyValue.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase);
                    }
                });
            }

            // Apply filters
            if (filter.VersionFilter != null) filteredSongs = filteredSongs.Where(s => s.Version == filter.VersionFilter);
            if (filter.ChapterFilter != null) filteredSongs = filteredSongs.Where(s => s.Chapter == filter.ChapterFilter);
            if (filter.ChartTypeFilter != null) filteredSongs = filteredSongs.Where(s => s.ChartType == filter.ChartTypeFilter);
            if (filter.LevelFilter != null) filteredSongs = filteredSongs.Where(s => s.Level == filter.LevelFilter);
            if (filter.IsMMOnly) filteredSongs = filteredSongs.Where(s => s.IsMM);
            if (filter.IsTP100Only) filteredSongs = filteredSongs.Where(s => s.TP == 100);
            if (filter.IsMxmOnly) filteredSongs = filteredSongs.Where(s => s.IsMxm);

            // Apply sorting
            if (filter.SortOption != SortOption.Default)
            {
                Func<C2SongViewModel, object> keySelector = filter.SortOption switch
                {
                    SortOption.Name => song => song.Name,
                    SortOption.Artist => song => song.Artist,
                    SortOption.Bpm => song => song.Bpm,
                    SortOption.Version => song => song.Version,
                    SortOption.ChartType => song => song.ChartType,
                    SortOption.Level => song => song.Level,
                    SortOption.LevelConstant => song => song.LevelConstant,
                    SortOption.Score => song => song.Score,
                    _ => throw new ArgumentOutOfRangeException(null)
                };
                if (filter.IsDescending)
                {
                    filteredSongs = filteredSongs.OrderByDescending(keySelector);
                }
                else
                {
                    filteredSongs = filteredSongs.OrderBy(keySelector);
                }
            }
            else
            {
                if (filter.IsDescending)
                {
                    filteredSongs = filteredSongs.Reverse();
                }
            }

            FilteredSongs = filteredSongs;
        }

#endregion

        #region Event Handlers
        private void C2SongViewModel_MMChanging(object sender, GenericPropertyChangingEventArgs<bool> e)
        {
            C2MMUndoableCommand command = new((C2SongViewModel)sender, e.OldValue, e.NewValue);
            undoStack.AddUndoCommand(command);
            UndoCommand.NotifyCanExecuteChanged();
            RedoCommand.NotifyCanExecuteChanged();
        }

        private void C2SongViewModel_TPChanging(object sender, GenericPropertyChangingEventArgs<decimal> e)
        {
            C2TPUndoableCommand command = new((C2SongViewModel)sender, e.OldValue, e.NewValue);
            undoStack.AddUndoCommand(command);
            UndoCommand.NotifyCanExecuteChanged();
            RedoCommand.NotifyCanExecuteChanged();
        }

        private void C2SongViewModel_MxmChanging(object sender, GenericPropertyChangingEventArgs<bool> e)
        {
            C2MxmUndoableCommand command = new((C2SongViewModel)sender, e.OldValue, e.NewValue);
            undoStack.AddUndoCommand(command);
            UndoCommand.NotifyCanExecuteChanged();
            RedoCommand.NotifyCanExecuteChanged();
        }

        private void C2SongViewModel_MMChanged(object sender, GenericPropertyChangedEventArgs<bool> e)
        {
            IsSaved = false;
            UpdateTotalScore();
        }

        private void C2SongViewModel_TPChanged(object sender, GenericPropertyChangedEventArgs<decimal> e)
        {
            IsSaved = false;
            UpdateTotalScore();
        }

        private void C2SongViewModel_MxmChanged(object sender, GenericPropertyChangedEventArgs<bool> e)
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
                Load(fileName);
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
                Save(FileName);
            }
        }

        void ExecuteSaveAs()
        {
            if (dialogService.ShowSaveFileDialog(out string fileName) == true)
            {
                Save(fileName);
            }
        }

        void ExecuteSettings()
        {
            SettingsDialogResult result = dialogService.ShowSettingsDialog();
            if (result.DialogResult)
            {
                C2SettingService.Instance.Language = result.Language;
                C2SettingService.Instance.StartAction = result.StartAction;
                C2SettingService.Instance.HighlightsOutlyingLevelConstants = result.HighlightsOutlyingLevelConstants;
                C2SettingService.Instance.HighlightsTopSongs = result.HighlightsTopSongs;
                C2SettingService.Instance.CascadesAchievements = result.CascadesAchievements;

                // Save settings
                using C2RegistryService reg = new();
                reg.SetSetting("Language", (int)result.Language);
                reg.SetSetting("StartAction", (int)result.StartAction);
                reg.SetSetting("HighlightsOutlyingLevelConstants", result.HighlightsOutlyingLevelConstants);
                reg.SetSetting("HighlightsTopSongs", result.HighlightsTopSongs);
                reg.SetSetting("CascadesAchievements", result.CascadesAchievements);

                RefreshListViewRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        void ExecuteUndo()
        {
            undoStack.Undo();
            UndoCommand.NotifyCanExecuteChanged();
            RedoCommand.NotifyCanExecuteChanged();
            UpdateTotalScore();
        }

        void ExecuteRedo()
        {
            undoStack.Redo();
            UndoCommand.NotifyCanExecuteChanged();
            RedoCommand.NotifyCanExecuteChanged();
            UpdateTotalScore();
        }

        void ExecuteCut()
        {
            ExecuteCopy();
            ExecuteDeleteSelection();
        }

        void ExecuteCopy()
        {
            clipboard = [.. SelectedSongs.Select(song => new C2ClipboardField() { IsMM = song.IsMM, TP = song.TP, IsMxm = song.IsMxm })];
            PasteCommand.NotifyCanExecuteChanged();
        }

        void ExecutePaste()
        {
            int index = 0;
            UndoableCommandCollection commands = [];
            foreach (var song in SelectedSongs)
            {
                var oldMM = song.IsMM;
                var oldTP = song.TP;
                var oldMxm = song.IsMxm;

                var newMM = clipboard[index % clipboard.Length].IsMM;
                var newTP = clipboard[index % clipboard.Length].TP;
                var newMxm = clipboard[index % clipboard.Length].IsMxm;

                if (oldMM != newMM)
                {
                    song.SetMM(newMM, SetPropertyOption.Silent);
                    commands.Add(new C2MMUndoableCommand(song, oldMM, newMM));
                }
                if (oldTP != newTP)
                {
                    song.SetTP(newTP, SetPropertyOption.Silent);
                    commands.Add(new C2TPUndoableCommand(song, oldTP, newTP));
                }
                if (oldMxm != newMxm)
                {
                    song.SetMxm(newMxm, SetPropertyOption.Silent);
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
                UpdateTotalScore();
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
            dialogService.ShowStatisticsDialog(Songs);
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

        // Constants
        const string PATH_SONGS_JSON = @".\data\songs.json";
        const string PATH_RANKS_JSON = @".\data\ranks.json";
        const string PATH_DROPDOWN_JSON = @".\data\dropdownitems.json";
        const string PATH_CHECKSUM = @".\data\checksum.dat";

        const int TARGET_LEVEL = 14;
    }

    readonly struct C2ClipboardField
    {
        public bool IsMM { get; init; }
        public decimal TP { get; init; }
        public bool IsMxm { get; init; }
    }
}
