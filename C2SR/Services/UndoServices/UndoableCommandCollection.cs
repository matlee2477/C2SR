using System.Collections;

namespace C2SR.Services.UndoServices
{
    public class UndoableCommandCollection : IUndoableCommand, ICollection<IUndoableCommand>
    {
        public UndoableCommandCollection()
        {
            commands = [];
        }

        // Fields
        readonly List<IUndoableCommand> commands;

        // Properties
        public int Count => commands.Count;

        public bool IsReadOnly => false;

        // IUndoableCommand
        public void Execute()
        {
            foreach (var command in commands)
            {
                command.Execute();
            }
        }

        public void Unexecute()
        {
            foreach (var command in Enumerable.Reverse(commands))
            {
                command.Unexecute();
            }
        }

        // ICollection
        public void Add(IUndoableCommand item)
        {
            commands.Add(item);
        }

        public bool Remove(IUndoableCommand item)
        {
            return commands.Remove(item);
        }

        public void Clear()
        {
            commands.Clear();
        }

        bool ICollection<IUndoableCommand>.Contains(IUndoableCommand item)
        {
            return commands.Contains(item);
        }

        void ICollection<IUndoableCommand>.CopyTo(IUndoableCommand[] array, int arrayIndex)
        {
            commands.CopyTo(array, arrayIndex);
        }

        IEnumerator<IUndoableCommand> IEnumerable<IUndoableCommand>.GetEnumerator()
        {
            return commands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return commands.GetEnumerator();
        }
    }
}
