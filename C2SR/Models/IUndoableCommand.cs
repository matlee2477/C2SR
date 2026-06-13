namespace C2SR.Models
{
    public interface IUndoableCommand
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        void Execute();

        /// <summary>
        /// Reverts the effects of Execute.
        /// </summary>
        void Unexecute();
    }
}
