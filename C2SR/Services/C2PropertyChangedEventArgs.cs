namespace C2SR.Services
{
    abstract class C2PropertyChangedEventArgs : EventArgs { }

    delegate void C2MMChangedEventHandler(object sender, C2MMChangedEventArgs e);
    class C2MMChangedEventArgs : C2PropertyChangedEventArgs
    {
        public C2MMChangedEventArgs(bool newValue)
        {
            NewValue = newValue;
        }

        // Properties
        public bool NewValue { get; }
    }

    delegate void C2TPChangedEventHandler(object sender, C2TPChangedEventArgs e);
    class C2TPChangedEventArgs : C2PropertyChangedEventArgs
    {
        public C2TPChangedEventArgs(decimal newValue)
        {
            NewValue = newValue;
        }

        // Properties
        public decimal NewValue { get; }
    }

    delegate void C2MxmChangedEventHandler(object sender, C2MxmChangedEventArgs e);
    class C2MxmChangedEventArgs : C2PropertyChangedEventArgs
    {
        public C2MxmChangedEventArgs(bool newValue)
        {
            NewValue = newValue;
        }

        // Properties
        public bool NewValue { get; }
    }

    delegate void C2MMChangingEventHandler(object sender, C2MMChangingEventArgs e);
    class C2MMChangingEventArgs : C2PropertyChangedEventArgs
    {
        public C2MMChangingEventArgs(bool oldValue, bool newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        // Properties
        public bool OldValue { get; }
        public bool NewValue { get; }
    }

    delegate void C2TPChangingEventHandler(object sender, C2TPChangingEventArgs e);
    class C2TPChangingEventArgs : C2PropertyChangedEventArgs
    {
        public C2TPChangingEventArgs(decimal oldValue, decimal newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        // Properties
        public decimal OldValue { get; }
        public decimal NewValue { get; }
    }

    delegate void C2MxmChangingEventHandler(object sender, C2MxmChangingEventArgs e);
    class C2MxmChangingEventArgs : C2PropertyChangedEventArgs
    {
        public C2MxmChangingEventArgs(bool oldValue, bool newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        // Properties
        public bool OldValue { get; }
        public bool NewValue { get; }
    }
}
