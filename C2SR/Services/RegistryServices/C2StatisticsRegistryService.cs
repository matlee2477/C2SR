namespace C2SR.Services.RegistryServices
{
    class C2StatisticsRegistryService : C2RegistryService
    {
        public C2StatisticsRegistryService() : base(@"Software\Cytus II Skill Rate\StatisticsWindow", string.Empty, 800, 450, 300, 300) { }
    }
}
