using System;
using System.Collections.Generic;
using System.ComponentModel;
using CpbTiming.SmsTiming;

namespace GoKart
{
    public partial class MainWindow
    {
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Position"))
            {
                UpdatePosition = true;
            }

            //try
            //{
            //    Console.WriteLine("Set " + e.PropertyName + "=" + sender.GetType().GetProperty(e.PropertyName).GetValue(sender)?.ToString());
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Set " + ex.Message);
            //}
        }
    }
}