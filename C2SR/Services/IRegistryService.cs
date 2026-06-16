namespace C2SR.Services
{
    interface IRegistryService : IDisposable
    {
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public int WindowLeft { get; set; }
        public int WindowTop { get; set; }
        public bool IsMaximized { get; set; }
        public string LastFileName { get; set; }

        public bool GetSetting(string name, bool defaultValue);
        public int GetSetting(string name, int defaultValue);
        public long GetSetting(string name, long defaultValue);
        public string GetSetting(string name, string defaultValue);
        public byte[] GetSetting(string name, byte[] defaultValue);
        public void SetSetting(string name, bool value);
        public void SetSetting(string name, int value);
        public void SetSetting(string name, long value);
        public void SetSetting(string name, string value);
        public void SetSetting(string name, byte[] value);
    }
}
