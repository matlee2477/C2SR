using Microsoft.Win32;

namespace C2SR.Services.RegistryServices
{
    class RegistryService : IRegistryService, IDisposable
    {
        public RegistryService() : this(@"Software\Cytus II Skill Rate", "Settings", 800, 450, 200, 200) { }

        protected RegistryService(string mainKeyPath, string settingsKeyPath, int defaultWindowWidth, int defaultWindowHeight, int defaultWindowLeft, int defaultWindowTop)
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
                ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
                return (int)(mainKey.GetValue("Width") ?? defaultWindowWidth);
            }
            set
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
                mainKey.SetValue("Width", value, RegistryValueKind.DWord);
            }
        }

        public int WindowHeight
        {
            get
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
                return (int)(mainKey.GetValue("Height") ?? defaultWindowHeight);
            }
            set
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
                mainKey.SetValue("Height", value, RegistryValueKind.DWord);
            }
        }

        public int WindowLeft
        {
            get
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
                return (int)(mainKey.GetValue("Left") ?? defaultWindowLeft);
            }
            set
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
                mainKey.SetValue("Left", value, RegistryValueKind.DWord);
            }
        }

        public int WindowTop
        {
            get
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
                return (int)(mainKey.GetValue("Top") ?? defaultWindowTop);
            }
            set
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
                mainKey.SetValue("Top", value, RegistryValueKind.DWord);
            }
        }

        public bool IsMaximized
        {
            get
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
                return Convert.ToBoolean((int)(mainKey.GetValue("IsMaximized") ?? 0));
            }
            set
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
                mainKey.SetValue("IsMaximized", Convert.ToInt32(value), RegistryValueKind.DWord);
            }
        }

        public string LastFileName
        {
            get
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
                return (string)(mainKey.GetValue("LastFileName") ?? string.Empty);
            }
            set
            {
                ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
                mainKey.SetValue("LastFileName", value, RegistryValueKind.String);
            }
        }

        #endregion

        #region Methods
        public bool GetVisibility(string name, bool defaultValue)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));

            if (mainKey.GetValue(name) is int value)
            {
                return Convert.ToBoolean(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public int GetSize(string name, int defaultValue)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));

            if (mainKey.GetValue(name) is int value)
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        public bool GetSetting(string name, bool defaultValue)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));

            if (settingsKey.GetValue(name) is int value)
            {
                return Convert.ToBoolean(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public int GetSetting(string name, int defaultValue)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));

            if (settingsKey.GetValue(name) is int value)
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        public long GetSetting(string name, long defaultValue)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));

            if (settingsKey.GetValue(name) is long value)
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        public string GetSetting(string name, string defaultValue)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));

            if (settingsKey.GetValue(name) is string value)
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        public byte[] GetSetting(string name, byte[] defaultValue)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));

            if (settingsKey.GetValue(name) is byte[] value)
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        public void SetVisibility(string name, bool value)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
            mainKey.SetValue(name, value, RegistryValueKind.DWord);
        }

        public void SetSize(string name, int value)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
            mainKey.SetValue(name, value, RegistryValueKind.DWord);
        }

        public void SetSetting(string name, bool value)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
            settingsKey.SetValue(name, value, RegistryValueKind.DWord);
        }

        public void SetSetting(string name, int value)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
            settingsKey.SetValue(name, value, RegistryValueKind.DWord);
        }

        public void SetSetting(string name, long value)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
            settingsKey.SetValue(name, value, RegistryValueKind.QWord);
        }

        public void SetSetting(string name, string value)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
            settingsKey.SetValue(name, value, RegistryValueKind.String);
        }

        public void SetSetting(string name, byte[] value)
        {
            ObjectDisposedException.ThrowIf(isDisposed, typeof(RegistryService));
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

        ~RegistryService()
        {
            Dispose(false);
        }

        #endregion
    }
}
