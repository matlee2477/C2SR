namespace C2SR.EventHandling
{
    public delegate void GenericPropertyChangedEventHandler<T>(object sender, GenericPropertyChangedEventArgs<T> e);
    public class GenericPropertyChangedEventArgs<T> : EventArgs
    {
        public GenericPropertyChangedEventArgs(T newValue)
        {
            NewValue = newValue;
        }

        // Properties
        public T NewValue { get; }
    }

    public delegate void GenericPropertyChangingEventHandler<T>(object sender, GenericPropertyChangingEventArgs<T> e);
    public class GenericPropertyChangingEventArgs<T> : GenericPropertyChangedEventArgs<T>
    {
        public GenericPropertyChangingEventArgs(T oldValue, T newValue) : base(newValue)
        {
            OldValue = oldValue;
        }

        // Properties
        public T OldValue { get; }
    }
}
