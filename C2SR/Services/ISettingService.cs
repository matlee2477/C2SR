namespace C2SR.Services
{
    interface ISettingService
    {
        public C2Language Language { get; set; }
        public C2StartAction StartAction { get; set; }
    }
}
