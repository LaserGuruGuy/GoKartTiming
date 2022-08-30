using System.Windows.Input;

namespace GoKart
{
    public partial class MainWindow
    {
        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
        
            dlg.Multiselect = true;
            dlg.DefaultExt = ".pdf";
            dlg.Filter = "CPB (*.pdf)|*.pdf|CPB (*.json)|*.json";

            if (dlg.ShowDialog() == true)
            {
                HandleFile(dlg.FileNames);
            }
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}