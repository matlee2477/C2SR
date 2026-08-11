using System.IO;
using System.Security.Cryptography;

namespace C2SR.Services.ChecksumServices
{
    class ChecksumService : IChecksumService
    {
        public ChecksumService()
        {
            TargetFiles = [];
        }

        // Properties
        public string[] TargetFiles { get; set; }

        // Methods
        public void CreateChecksum(string checksumFilePath)
        {
            List<byte> bytes = [];
            foreach (var file in TargetFiles)
            {
                using var stream = File.OpenRead(file);
                bytes.AddRange(SHA256.HashData(stream));
            }

            using var checksumFileStream = File.Create(checksumFilePath);
            checksumFileStream.Write([.. bytes]);
        }

        public bool VerifyChecksum(string checksumFilePath)
        {
            if (!File.Exists(checksumFilePath))
            {
                return false;
            }

            using var checksumFileStream = File.OpenRead(checksumFilePath);
            using var reader = new BinaryReader(checksumFileStream);
            foreach (var file in TargetFiles)
            {
                if (!File.Exists(file))
                {
                    return false;
                }

                using var stream = File.OpenRead(file);
                var computedHash = SHA256.HashData(stream);
                var storedHash = reader.ReadBytes(computedHash.Length);
                if (!computedHash.SequenceEqual(storedHash))
                {
                    return false;
                }
            }

            return checksumFileStream.Position == checksumFileStream.Length;
        }
    }
}
