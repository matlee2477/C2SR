namespace C2SR.Services.ChecksumServices
{
    interface IChecksumService
    {
        // Properties
        public string[] TargetFiles { get; set; }

        // Methods
        public void CreateChecksum(string checksumFilePath);
        public bool VerifyChecksum(string checksumFilePath);
    }
}
