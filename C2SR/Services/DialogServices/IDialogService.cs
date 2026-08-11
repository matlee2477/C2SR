using System.Windows;

namespace C2SR.Services.DialogServices
{
    interface IDialogService<TInput, TOutput> where TInput : class, IDialogServiceInput
                                              where TOutput : class, IDialogServiceOutput
    {
        public Window Owner { get; set; }
        public bool? ShowDialog(TInput input, out TOutput output);
    }

    interface IDialogServiceInput { }
    interface IDialogServiceOutput { }
}
