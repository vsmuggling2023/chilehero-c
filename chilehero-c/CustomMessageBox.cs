using System.Windows.Forms;

namespace chilehero_c
{
    public static class CustomMessageBox
    {

        public static DialogResult Show(
            string message,
            string title = "Mensaje",
            CustomMessageBoxType type = CustomMessageBoxType.Info,
            bool showCancel = false,
            IWin32Window owner = null)
        {
            using (var f = new CustomMessageBoxForm(message, title, type, showCancel))
            {
                return owner != null ? f.ShowDialog(owner) : f.ShowDialog();
            }
        }

        public static DialogResult Info(string message, string title = "Info", IWin32Window owner = null)
        {
            return Show(message, title, CustomMessageBoxType.Info, false, owner);
        }

        public static DialogResult Warning(string message, string title = "Aviso", IWin32Window owner = null)
        {
            return Show(message, title, CustomMessageBoxType.Warning, false, owner);
        }

        public static DialogResult Error(string message, string title = "Error", IWin32Window owner = null)
        {
            return Show(message, title, CustomMessageBoxType.Error, false, owner);
        }

        public static DialogResult Confirm(string message, string title = "Confirmar", IWin32Window owner = null)
        {
            return Show(message, title, CustomMessageBoxType.Warning, true, owner);
        }

        public static DialogResult Success(string message, string title = "Éxito", IWin32Window owner = null)
        {
            return Show(message, title, CustomMessageBoxType.Success, false, owner);
        }
    }
}