using Microsoft.Win32;

namespace C2SR.Services.RegistryServices
{
    class C2RegistryService : IRegistryService, IDisposable
    {
        public C2RegistryService() : this(@"Software\Cytus II Skill Rate", "Settings", 800, 450, 200, 200) { }

        protected C2RegistryService(string mainKeyPath, string settingsKeyPath, int defaultWindowWidth, int defaultWindowHeight, int defaultWindowLeft, int defaultWindowTop)
        {
            mainKey = Registry.CurrentUser.CreateSubKey(mainKeyPath);
            if (!string.IsNullOrEmpty(settingsKeyPath)) settingsKey = mainKey.CreateSubKey(settingsKeyPath); else settingsKey = mainKey;
            this.defaultWindowWidth = defaultWindowWidth;
            this.defaultWindowHeight = defaultWindowHeight;
            this.defaultWindowLeft = defaultWindowLeft;
            this.defaultWindowTop = defaultWindowTop;
            isDisposed = false;
        }

        // Fields
        protected readonly RegistryKey mainKey;
        protected readonly RegistryKey settingsKey;

        protected readonly int defaultWindowWidth;
        protected readonly int defaultWindowHeight;
        protected readonly int defaultWindowLeft;
        protected readonly int defaultWindowTop;

        #region Properties
        public int WindowWidth
        {
            get
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
                return (int)(mainKey.GetValue("Width") ?? defaultWindowWidth);
            }
            set
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
                mainKey.SetValue("Width", value, RegistryValueKind.DWord);
            }
        }

        public int WindowHeight
        {
            get
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
                return (int)(mainKey.GetValue("Height") ?? defaultWindowHeight);
            }
            set
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
                mainKey.SetValue("Height", value, RegistryValueKind.DWord);
            }
        }

        public int WindowLeft
        {
            get
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
                return (int)(mainKey.GetValue("Left") ?? defaultWindowLeft);
            }
            set
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
                mainKey.SetValue("Left", value, RegistryValueKind.DWord);
            }
        }

        public int WindowTop
        {
            get
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
                return (int)(mainKey.GetValue("Top") ?? defaultWindowTop);
            }
            set
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
                mainKey.SetValue("Top", value, RegistryValueKind.DWord);
            }
        }

        public bool IsMaximized
        {
            get
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
                return Convert.ToBoolean((int)(mainKey.GetValue("IsMaximized") ?? 0));
            }
            set
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
                mainKey.SetValue("IsMaximized", Convert.ToInt32(value), RegistryValueKind.DWord);
            }
        }

        public string LastFileName
        {
            get
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
                return (string)(mainKey.GetValue("LastFileName") ?? string.Empty);
            }
            set
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
                mainKey.SetValue("LastFileName", value, RegistryValueKind.String);
            }
        }

        #endregion

        #region Methods
        public bool GetSetting(string name, bool defaultValue)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
            return (bool)(settingsKey.GetValue(name) ?? defaultValue);
        }

        public int GetSetting(string name, int defaultValue)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
            return (int)(settingsKey.GetValue(name) ?? defaultValue);
        }

        public long GetSetting(string name, long defaultValue)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
            return (long)(settingsKey.GetValue(name) ?? defaultValue);
        }

        public string GetSetting(string name, string defaultValue)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
            return (string)(settingsKey.GetValue(name) ?? defaultValue);
        }

        public byte[] GetSetting(string name, byte[] defaultValue)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
            return (byte[])(settingsKey.GetValue(name) ?? defaultValue);
        }

        public void SetSetting(string name, bool value)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
            settingsKey.SetValue(name, value, RegistryValueKind.DWord);
        }

        public void SetSetting(string name, int value)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
            settingsKey.SetValue(name, value, RegistryValueKind.DWord);
        }

        public void SetSetting(string name, long value)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
            settingsKey.SetValue(name, value, RegistryValueKind.QWord);
        }

        public void SetSetting(string name, string value)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
            settingsKey.SetValue(name, value, RegistryValueKind.String);
        }

        public void SetSetting(string name, byte[] value)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(C2RegistryService));
            settingsKey.SetValue(name, value, RegistryValueKind.Binary);
        }

        public void Close() => Dispose();

        #endregion

        #region IDisposable
        bool isDisposed;

        void Dispose(bool isDisposing)
        {
            if (!isDisposed)
            {
                if (isDisposing)
                {
                    // Dispose managed resources
                    settingsKey.Close();
                    mainKey.Close();
                }
                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~C2RegistryService()
        {
            Dispose(false);
        }

        #endregion
    }
}
