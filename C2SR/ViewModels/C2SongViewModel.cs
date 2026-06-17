using C2SR.EventHandling;
using C2SR.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace C2SR.ViewModels
{
    public class C2SongViewModel : ObservableObject
    {
        public C2SongViewModel(C2Song song)
        {
            this.song = song;
            SkillRateFontWeight = FontWeights.Normal;
        }

        // Fields
        readonly C2Song song;

        #region Properties
        public long ID => song.ID;
        public string Name => song.Name;
        public string Artist => song.Artist;
        public decimal Bpm => song.Bpm;
        public string Version => song.Version;
        public string Chapter => song.Chapter;
        public string ChartType => song.ChartType;
        public decimal Level => song.Level;
        public decimal LevelConstant => song.LevelConstant;
        public decimal Score => song.Score;

        public bool IsMM
        {
            get => song.IsMM;
            set => SetMM(value);
        }

        public decimal TP
        {
            get => song.TP;
            set => SetTP(value);
        }

        public bool IsMxm
        {
            get => song.IsMxm;
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
            if (song.IsMM != isMM)
            {
                if (option != C2SongSetPropertyOption.Silent)
                {
                    OnMMChanging(song.IsMM, isMM);
                    song.IsMM = isMM;
                    OnMMChanged(isMM);
                }
                else
                {
                    OnPropertyChanging(nameof(IsMM));
                    song.IsMM = isMM;
                    OnPropertyChanged(nameof(IsMM));
                    OnPropertyChanged(nameof(Score));
                }
            }
        }

        public void SetTP(decimal tp, C2SongSetPropertyOption option)
        {
            if (song.TP != tp)
            {
                if (option != C2SongSetPropertyOption.Silent)
                {
                    OnTPChanging(song.TP, tp);
                    song.TP = tp;
                    OnTPChanged(tp);

                    if (tp == 100)
                    {
                        SetMM(true);
                    }
                }
                else
                {
                    OnPropertyChanging(nameof(TP));
                    song.TP = tp;
                    OnPropertyChanged(nameof(TP));
                    OnPropertyChanged(nameof(Score));
                }
            }
        }

        public void SetMxm(bool isMxm, C2SongSetPropertyOption option)
        {
            if (song.IsMxm != isMxm)
            {
                if (option != C2SongSetPropertyOption.Silent)
                {
                    OnMxmChanging(song.IsMxm, isMxm);
                    song.IsMxm = isMxm;
                    OnMxmChanged(isMxm);

                    if (isMxm)
                    {
                        SetTP(100);
                    }
                }
                else
                {
                    OnPropertyChanging(nameof(IsMxm));
                    song.IsMxm = isMxm;
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

    public enum C2SongSetPropertyOption
    {
        None = 0,
        Silent = 1,
    }
}
