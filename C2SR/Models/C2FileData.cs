namespace C2SR.Models
{
    readonly struct C2FileData
    {
        public long ID { get; init; }
        public bool IsMM { get; init; }
        public decimal TP { get; init; }
        public bool IsMxm { get; init; }
    }
}
