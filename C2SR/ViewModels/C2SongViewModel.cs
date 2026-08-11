using C2SR.EventHandling;
using C2SR.Models;
using C2SR.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Numerics;

namespace C2SR.ViewModels
{
    public class C2SongViewModel : ObservableObject
    {
        public C2SongViewModel(C2Song song)
        {
            this.song = song;
            IsTopSong = false;
        }

        // Fields
        readonly C2Song song;

        #region Properties
        public BigInteger ID => song.ID;
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

        public bool IsTopSong
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(IsTopSong));
            }
        }

        #endregion

        #region Events
        public event GenericPropertyChangingEventHandler<bool>? MMChanging;
        public event GenericPropertyChangedEventHandler<bool>? MMChanged;
        public event GenericPropertyChangingEventHandler<decimal>? TPChanging;
        public event GenericPropertyChangedEventHandler<decimal>? TPChanged;
        public event GenericPropertyChangingEventHandler<bool>? MxmChanging;
        public event GenericPropertyChangedEventHandler<bool>? MxmChanged;

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
        public override string ToString() => $"{Name}";

        public void SetMM(bool isMM, SetPropertyOption option)
        {
            if (song.IsMM != isMM)
            {
                if (option != SetPropertyOption.Silent)
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

        public void SetTP(decimal tp, SetPropertyOption option)
        {
            if (song.TP != tp)
            {
                if (option != SetPropertyOption.Silent)
                {
                    OnTPChanging(song.TP, tp);
                    song.TP = tp;
                    OnTPChanged(tp);

                    if (C2SettingService.Instance.CascadesAchievements && tp == 100)
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

        public void SetMxm(bool isMxm, SetPropertyOption option)
        {
            if (song.IsMxm != isMxm)
            {
                if (option != SetPropertyOption.Silent)
                {
                    OnMxmChanging(song.IsMxm, isMxm);
                    song.IsMxm = isMxm;
                    OnMxmChanged(isMxm);

                    if (C2SettingService.Instance.CascadesAchievements && isMxm)
                    {
                        SetTP(100);
                    }
                }
                else
                {
                    OnPropertyChanging(nameof(IsMxm));
                    song.IsMxm = isMxm;
                    OnPropertyChanged(nameof(IsMxm));
                }
            }
        }

        public void SetMM(bool isMM) => SetMM(isMM, SetPropertyOption.None);
        public void SetTP(decimal tp) => SetTP(tp, SetPropertyOption.None);
        public void SetMxm(bool isMxm) => SetMxm(isMxm, SetPropertyOption.None);

        #endregion
    }

    public enum SetPropertyOption
    {
        None = 0,
        Silent = 1,
    }
}
