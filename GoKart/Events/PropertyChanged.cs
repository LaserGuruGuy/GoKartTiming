using System;
using System.Collections.Generic;
using System.ComponentModel;
using CpbTiming.SmsTiming;

namespace GoKart
{
    public partial class MainWindow
    {
        private bool _UpdatePosition = false;

        public bool UpdatePosition
        {
            get
            {
                cacheLock.EnterReadLock();
                try
                {
                    return _UpdatePosition;
                }
                finally
                {
                    cacheLock.ExitReadLock();
                }
            }
            set
            {
                cacheLock.EnterWriteLock();
                try
                {
                    _UpdatePosition = value;
                }
                finally
                {
                    cacheLock.ExitWriteLock();
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Position"))
            {
                UpdatePosition = true;
            }

            try
            {
                Console.WriteLine("PropertyChanged " + e.PropertyName + "=" + sender.GetType().GetProperty(e.PropertyName)?.GetValue(sender)?.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}