using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using CpbTiming.SmsTiming;

namespace GoKart
{
    public partial class MainWindow
    {
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            string name = string.Empty;

            if (sender.GetType().Equals(typeof(UniqueObservableCollection<LiveTimingEx>)))
            {
                try
                {
                    UniqueObservableCollection<LiveTimingEx> collection = sender as UniqueObservableCollection<LiveTimingEx>;
                    LiveTimingEx element = collection[collection.Count - 1];
                    name += (name == string.Empty ? element.HeatName : "\n" + element.HeatName);
                }
                catch (Exception ex)
                {
                    name = ex.Message;
                }
            }
            else if (sender.GetType().Equals(typeof(UniqueObservableCollection<DriverEx>)))
            {
                try
                {
                    UniqueObservableCollection<DriverEx> collection = sender as UniqueObservableCollection<DriverEx>;
                    DriverEx element = collection[collection.Count - 1];
                    name += (name == string.Empty ? element.DriverName : "\n" + element.DriverName);
                }
                catch (Exception ex)
                {
                    name = ex.Message;
                }
            }
            else if (sender.GetType().Equals(typeof(UniqueObservableCollection<KeyValuePair<int, TimeSpan>>)))
            {
                UpdateLapTimeWindow = true;

                try
                {
                    UniqueObservableCollection<KeyValuePair<int, TimeSpan>> collection = sender as UniqueObservableCollection<KeyValuePair<int, TimeSpan>>;
                    KeyValuePair<int, TimeSpan> element = collection[collection.Count - 1];
                    name += (name == string.Empty ? element.Key.ToString() : "\n" + element.Value.ToString());
                }
                catch (Exception ex)
                {
                    name = ex.Message;
                }
            }

            Console.WriteLine(e.Action + " " + name);
        }
    }
}
