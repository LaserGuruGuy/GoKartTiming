﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using CpbTiming.SmsTiming;

namespace GoKart
{
    public partial class MainWindow
    {
        private bool _UpdateLiveTiming = false;
        private bool _UpdateDriver = false;
        private bool _UpdateLapTime = false;

        public bool UpdateLiveTiming
        {
            get
            {
                cacheLock.EnterReadLock();
                try
                {
                    return _UpdateLiveTiming;
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
                    _UpdateLiveTiming = value;
                }
                finally
                {
                    cacheLock.ExitWriteLock();
                }
            }
        }

        public bool UpdateDriver
        {
            get
            {
                cacheLock.EnterReadLock();
                try
                {
                    return _UpdateDriver;
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
                    _UpdateDriver = value;
                }
                finally
                {
                    cacheLock.ExitWriteLock();
                }
            }
        }

        public bool UpdateLapTime
        {
            get
            {
                cacheLock.EnterReadLock();
                try
                {
                    return _UpdateLapTime;
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
                    _UpdateLapTime = value;
                }
                finally
                {
                    cacheLock.ExitWriteLock();
                }
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            string name = string.Empty;

            if (sender.GetType().Equals(typeof(UniqueObservableCollection<LiveTimingEx>)))
            {
                UpdateLiveTiming = true;

                try
                {
                    UniqueObservableCollection<LiveTimingEx> collection = sender as UniqueObservableCollection<LiveTimingEx>;
                    LiveTimingEx element = collection[collection.Count - 1];
                    name = element.HeatName;
                }
                catch (Exception ex)
                {
                    name = ex.Message;
                }
            }
            else if (sender.GetType().Equals(typeof(UniqueObservableCollection<DriverEx>)))
            {
                UpdateDriver = true;

                try
                {
                    UniqueObservableCollection<DriverEx> collection = sender as UniqueObservableCollection<DriverEx>;
                    DriverEx element = collection[collection.Count - 1];
                    name = element.DriverName;
                }
                catch (Exception ex)
                {
                    name = ex.Message;
                }
            }
            else if (sender.GetType().Equals(typeof(UniqueObservableCollection<KeyValuePair<int, TimeSpan>>)))
            {
                UpdateLapTime = true;

                try
                {
                    UniqueObservableCollection<KeyValuePair<int, TimeSpan>> collection = sender as UniqueObservableCollection<KeyValuePair<int, TimeSpan>>;
                    KeyValuePair<int, TimeSpan> element = collection[collection.Count - 1];
                    name = element.Key.ToString() + " " + ((TimeSpan)(element.Value)).ToString();
                }
                catch (Exception ex)
                {
                    name = ex.Message;
                }
            }

            Console.WriteLine("CollectionChanged " + e.Action + " " + name);
        }
    }
}