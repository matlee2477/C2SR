namespace C2SR.Services.RegistryServices
{
    class StatisticsRegistryService : RegistryService
    {
        public StatisticsRegistryService() : base(@"Software\Cytus II Skill Rate\StatisticsWindow", string.Empty, 800, 450, 300, 300) { }
    }
}
