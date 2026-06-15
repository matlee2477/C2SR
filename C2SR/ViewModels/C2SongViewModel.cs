using C2SR.Models;
using C2SR.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace C2SR.ViewModels
{
    class C2SongViewModel : ObservableObject
    {
        public C2SongViewModel(C2Song song)
        {
            Song = song;
        }

        #region Properties
        public C2Song Song { get; }

        public string Name => Song.Name;
        public string Artist => Song.Artist;
        public string Bpm => Song.Bpm.ToString();
        public string Chapter => Song.Chapter;
        public string ChartType => Song.ChartType;
        public string Level => GetLevelString();
        public string LevelConstant => Song.LevelConstant.ToString("N1");
        public string SkillRate => Song.SkillRate.ToString("N2");

        public bool IsMM
        {
            get => Song.IsMM;
            set => SetMM(value);
        }

        public string TP
        {
            get => Song.TP.ToString("N2");
            set
            {
                if (decimal.TryParse(value, out decimal tp))
                {
                    if (tp < 0) tp = 0;
                    if (tp > 100) tp = 100;
                    SetTP(tp);
                }
            }
        }

        public bool IsMxm
        {
            get => Song.IsMxm;
            set => SetMxm(value);
        }

        public Brush LevelConstantBrush
        {
            get
            {
                return (Song.LevelConstant - Song.Level) switch
                {
                    >= 0.3M => new SolidColorBrush(Colors.Red),
                    <= -0.3M => new SolidColorBrush(Colors.Blue),
                    _ => new SolidColorBrush(Colors.Black)
                };
            }
        }

        public FontWeight LevelConstantFontWeight
        {
            get
            {
                return Math.Abs(Song.LevelConstant - Song.Level) switch
                {
                    >= 0.5M or <= -0.5M => FontWeights.Bold,
                    _ => FontWeights.Normal
                };
            }
        }

        public FontWeight SkillRateFontWeight { get; set; } = FontWeights.Normal;

        #endregion

        #region Events
        public event C2MMChangingEventHandler? MMChanging;
        public event C2MMChangedEventHandler? MMChanged;
        public event C2TPChangingEventHandler? TPChanging;
        public event C2TPChangedEventHandler? TPChanged;
        public event C2MxmChangingEventHandler? MxmChanging;
        public event C2MxmChangedEventHandler? MxmChanged;

        protected void OnMMChanging(bool oldValue, bool newValue)
        {
            MMChanging?.Invoke(this, new(oldValue, newValue));
            OnPropertyChanging(nameof(IsMM));
        }
        protected void OnMMChanged(bool newValue)
        {
            MMChanged?.Invoke(this, new(newValue));
            OnPropertyChanged(nameof(IsMM));
            OnPropertyChanged(nameof(SkillRate));
        }

        protected void OnTPChanging(decimal oldValue, decimal newValue)
        {
            TPChanging?.Invoke(this, new(oldValue, newValue));
            OnPropertyChanging(nameof(TP));
        }

        protected void OnTPChanged(decimal newValue)
        {
            TPChanged?.Invoke(this, new(newValue));
            OnPropertyChanged(nameof(TP));
            OnPropertyChanged(nameof(SkillRate));
        }

        protected void OnMxmChanging(bool oldValue, bool newValue)
        {
            MxmChanging?.Invoke(this, new(oldValue, newValue));
            OnPropertyChanging(nameof(IsMxm));
        }

        protected void OnMxmChanged(bool newValue)
        {
            MxmChanged?.Invoke(this, new(newValue));
            OnPropertyChanged(nameof(IsMxm));
        }

        #endregion

        #region Methods
        private string GetLevelString()
        {
            StringBuilder sb = new();
            sb.Append(Song.Level.ToString("N0"));
            if (Math.Floor(Song.Level) != Song.Level) sb.Append('+');
            return sb.ToString();
        }

        public void SetMM(bool isMM, C2SongSetPropertyOption option)
        {
            if (Song.IsMM != isMM)
            {
                if (option != C2SongSetPropertyOption.Silent)
                {
                    OnMMChanging(Song.IsMM, isMM);
                    Song.IsMM = isMM;
                    OnMMChanged(isMM);
                }
                else
                {
                    OnPropertyChanging(nameof(IsMM));
                    Song.IsMM = isMM;
                    OnPropertyChanged(nameof(IsMM));
                    OnPropertyChanged(nameof(SkillRate));
                }
            }
        }

        public void SetTP(decimal tp, C2SongSetPropertyOption option)
        {
            if (Song.TP != tp)
            {
                if (option != C2SongSetPropertyOption.Silent)
                {
                    OnTPChanging(Song.TP, tp);
                    Song.TP = tp;
                    OnTPChanged(tp);

                    if (tp == 100)
                    {
                        SetMM(true);
                    }
                }
                else
                {
                    OnPropertyChanging(nameof(TP));
                    Song.TP = tp;
                    OnPropertyChanged(nameof(TP));
                    OnPropertyChanged(nameof(SkillRate));
                }
            }
        }

        public void SetMxm(bool isMxm, C2SongSetPropertyOption option)
        {
            if (Song.IsMxm != isMxm)
            {
                if (option != C2SongSetPropertyOption.Silent)
                {
                    OnMxmChanging(Song.IsMxm, isMxm);
                    Song.IsMxm = isMxm;
                    OnMxmChanged(isMxm);

                    if (isMxm)
                    {
                        SetTP(100);
                    }
                }
                else
                {
                    OnPropertyChanging(nameof(IsMxm));
                    Song.IsMxm = isMxm;
                    OnPropertyChanged(nameof(IsMxm));
                    OnPropertyChanged(nameof(SkillRate));
                }
            }
        }

        public void SetMM(bool isMM) => SetMM(isMM, C2SongSetPropertyOption.None);
        public void SetTP(decimal tp) => SetTP(tp, C2SongSetPropertyOption.None);
        public void SetMxm(bool isMxm) => SetMxm(isMxm, C2SongSetPropertyOption.None);

        #endregion
    }

    enum C2SongSetPropertyOption
    {
        None = 0,
        Silent = 1,
    }
}
