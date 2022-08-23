using System.Windows;
using System.Windows.Input;

namespace GoKart
{
    public partial class MainWindow : Window
    {
        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
        
            dlg.Multiselect = true;
            dlg.DefaultExt = ".pdf";
            dlg.Filter = "CPB (*.pdf)|*.pdf";

            if (dlg.ShowDialog() == true)
            {
                ParseFiles(dlg.FileNames);
            }
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}