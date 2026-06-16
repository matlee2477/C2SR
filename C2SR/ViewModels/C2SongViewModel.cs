using C2SR.Models;
using C2SR.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace C2SR.ViewModels
{
    class C2SongViewModel : ObservableObject
    {
        public C2SongViewModel(C2Song song)
        {
            Song = song;
            SkillRateFontWeight = FontWeights.Normal;
        }

        #region Properties
        public C2Song Song { get; }

        public string Name => Song.Name;
        public string Artist => Song.Artist;
        public decimal Bpm => Song.Bpm;
        public string Version => Song.Version;
        public string Chapter => Song.Chapter;
        public string ChartType => Song.ChartType;
        public decimal Level => Song.Level;
        public decimal LevelConstant => Song.LevelConstant;
        public decimal Score => Song.Score;

        public bool IsMM
        {
            get => Song.IsMM;
            set => SetMM(value);
        }

        public decimal TP
        {
            get => Song.TP;
            set => SetTP(value);
        }

        public bool IsMxm
        {
            get => Song.IsMxm;
            set => SetMxm(value);
        }

        public FontWeight SkillRateFontWeight
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(SkillRateFontWeight));
            }
        }

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
            OnPropertyChanged(nameof(Score));
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
            OnPropertyChanged(nameof(Score));
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
                    OnPropertyChanged(nameof(Score));
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
                    OnPropertyChanged(nameof(Score));
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
                    OnPropertyChanged(nameof(Score));
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
