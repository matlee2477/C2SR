using System.Windows;

namespace C2SR.Services.DialogServices
{
    abstract class DialogService<TInput, TOutput> : IDialogService<TInput, TOutput> where TInput : class, IDialogServiceInput
                                                                                    where TOutput : class, IDialogServiceOutput
    {
        public DialogService()
        {
            Owner = Application.Current.MainWindow;
        }

        // Properties
        public Window Owner { get; set; }

        // Methods
        public abstract bool? ShowDialog(TInput input, out TOutput output);
    }
}
