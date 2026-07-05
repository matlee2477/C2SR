namespace C2SR.Exceptions
{
    public class ChecksumMismatchException : Exception
    {
        public ChecksumMismatchException() : base("Checksum mismatch detected. Some of the essential files may be corrupted.") { }
        public ChecksumMismatchException(string? message) : base(message) { }
        public ChecksumMismatchException(string? message, Exception? innerException) : base(message, innerException) { }

        // Methods
        public static void ThrowIfChecksumMismatch(Func<bool> verifyChecksumCall)
        {
            ArgumentNullException.ThrowIfNull(verifyChecksumCall);

            if (!verifyChecksumCall())
            {
                throw new ChecksumMismatchException();
            }
        }
    }
}
