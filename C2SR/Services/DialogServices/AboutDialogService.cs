using C2SR.Views;

namespace C2SR.Services.DialogServices
{
    class AboutDialogService : SimpleDialogService
    {
        public AboutDialogService() : base() { }

        // Methods
        public override void ShowDialog()
        {
            AboutDialog dialog = new() { Owner = Owner };
            dialog.ShowDialog();
        }
    }
}
