using System;
using System.Windows.Input;

namespace GoKart
{
    public partial class MainWindow
    {
        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog OpenFileDialog = new Microsoft.Win32.OpenFileDialog();

            OpenFileDialog.InitialDirectory = LocalApplicationDataFolder;
            OpenFileDialog.Multiselect = true;
            OpenFileDialog.DefaultExt = ".json";
            OpenFileDialog.Filter = "(*.json)|*.json|(*.pdf)|*.pdf";

            if (OpenFileDialog.ShowDialog() == true)
            {
                HandleFile(OpenFileDialog.FileNames);
            }
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}