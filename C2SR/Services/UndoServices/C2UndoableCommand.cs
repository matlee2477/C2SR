using C2SR.ViewModels;

namespace C2SR.Services.UndoServices
{
    abstract class C2UndoableCommand : IUndoableCommand
    {
        public C2UndoableCommand(C2SongViewModel song)
        {
            Song = song;
        }

        // Properties
        public C2SongViewModel Song { get; }

        // Methods
        public abstract void Execute();
        public abstract void Unexecute();
    }

    class C2MMUndoableCommand : C2UndoableCommand
    {
        public C2MMUndoableCommand(C2SongViewModel song, bool oldValue, bool newValue) : base(song)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        // Properties
        public bool OldValue { get; }
        public bool NewValue { get; }

        // Methods
        public override void Execute() => Song.SetMM(NewValue, SetPropertyOption.Silent);
        public override void Unexecute() => Song.SetMM(OldValue, SetPropertyOption.Silent);
    }

    class C2TPUndoableCommand : C2UndoableCommand
    {
        public C2TPUndoableCommand(C2SongViewModel song, decimal oldValue, decimal newValue) : base(song)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        // Properties
        public decimal OldValue { get; }
        public decimal NewValue { get; }

        // Methods
        public override void Execute() => Song.SetTP(NewValue, SetPropertyOption.Silent);
        public override void Unexecute() => Song.SetTP(OldValue, SetPropertyOption.Silent);
    }

    class C2MxmUndoableCommand : C2UndoableCommand
    {
        public C2MxmUndoableCommand(C2SongViewModel song, bool oldValue, bool newValue) : base(song)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        // Properties
        public bool OldValue { get; }
        public bool NewValue { get; }

        // Methods
        public override void Execute() => Song.SetMxm(NewValue, SetPropertyOption.Silent);
        public override void Unexecute() => Song.SetMxm(OldValue, SetPropertyOption.Silent);
    }
}
