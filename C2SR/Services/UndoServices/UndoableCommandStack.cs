namespace C2SR.Services.UndoServices
{
    public class UndoableCommandStack
    {
        public UndoableCommandStack()
        {
            undoStack = [];
            redoStack = [];
        }

        // Fields
        readonly Stack<IUndoableCommand> undoStack;
        readonly Stack<IUndoableCommand> redoStack;

        // Properties
        public bool CanUndo => undoStack.Count > 0;
        public bool CanRedo => redoStack.Count > 0;

        // Methods
        public void AddUndoCommand(IUndoableCommand command)
        {
            undoStack.Push(command);
            redoStack.Clear();
        }

        public void Undo()
        {
            if (CanUndo)
            {
                var command = undoStack.Pop();
                command.Unexecute();
                redoStack.Push(command);
            }
        }

        public void Redo()
        {
            if (CanRedo)
            {
                var command = redoStack.Pop();
                command.Execute();
                undoStack.Push(command);
            }
        }

        public void Clear()
        {
            undoStack.Clear();
            redoStack.Clear();
        }

        // Singleton
        static readonly Lazy<UndoableCommandStack> lazy = new(() => new());
        public static UndoableCommandStack Instance => lazy.Value;
    }
}
