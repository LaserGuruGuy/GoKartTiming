using System;
using System.ComponentModel;

namespace GoKart
{
    public partial class MainWindow
    {
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                Console.WriteLine("Set " + e.PropertyName + "=" + sender.GetType().GetProperty(e.PropertyName).GetValue(sender)?.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Set " + ex.Message);
            }
        }
    }
}