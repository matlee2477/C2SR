namespace C2SR.Models
{
    public interface IUndoableCommand
    {
        // Executes the command.
        void Execute();

        // Reverts the effects of Execute.
        void Unexecute();
    }
}
