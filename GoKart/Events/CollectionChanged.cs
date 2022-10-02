using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using GoKartTiming.BestTiming;
using GoKartTiming.LiveTiming;

namespace GoKart
{
    public partial class MainWindow
    {
        private bool _UpdateLiveTiming = false;
        private bool _UpdateDriver = false;
        private bool _UpdateLapTime = false;
        private bool _UpdateScoreGroup = false;
        private bool _UpdateRecordGroup = false;

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

        public bool UpdateScoreGroup
        {
            get
            {
                cacheLock.EnterReadLock();
                try
                {
                    return _UpdateScoreGroup;
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
                    _UpdateScoreGroup = value;
                }
                finally
                {
                    cacheLock.ExitWriteLock();
                }
            }
        }

        public bool UpdateRecordGroup
        {
            get
            {
                cacheLock.EnterReadLock();
                try
                {
                    return _UpdateRecordGroup;
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
                    _UpdateRecordGroup = value;
                }
                finally
                {
                    cacheLock.ExitWriteLock();
                }
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
#if DEBUG
            string name = string.Empty;
#endif
            if (sender.GetType().Equals(typeof(ObservableCollection<ScoreGroup>)))
            {
                UpdateScoreGroup = true;
#if DEBUG
                try
                {
                    ObservableCollection<ScoreGroup> collection = sender as ObservableCollection<ScoreGroup>;
                    if (collection.Count > 0)
                    {
                        ScoreGroup element = collection[collection.Count - 1];
                        name = element.scoreGroupName;
                    }
                }
                catch (Exception ex)
                {
                    name = ex.Message;
                }
#endif
            }
            else if (sender.GetType().Equals(typeof(ObservableCollection<RecordGroup>)))
            {
                UpdateRecordGroup = true;
#if DEBUG
                try
                {
                    ObservableCollection<RecordGroup> collection = sender as ObservableCollection<RecordGroup>;
                    if (collection.Count > 0)
                    {
                        RecordGroup element = collection[collection.Count - 1];
                        name = element.Participant;
                    }
                }
                catch (Exception ex)
                {
                    name = ex.Message;
                }
#endif
            }
            else if (sender.GetType().Equals(typeof(UniqueObservableCollection<LiveTimingEx>)))
            {
                UpdateLiveTiming = true;
#if DEBUG
                try
                {
                    UniqueObservableCollection<LiveTimingEx> collection = sender as UniqueObservableCollection<LiveTimingEx>;
                    if (collection.Count > 0)
                    {
                        LiveTimingEx element = collection[collection.Count - 1];
                        name = element.HeatName;
                    }
                }
                catch (Exception ex)
                {
                    name = ex.Message;
                }
#endif
            }
            else if (sender.GetType().Equals(typeof(UniqueObservableCollection<DriverEx>)))
            {
                UpdateDriver = true;
#if DEBUG
                try
                {
                    UniqueObservableCollection<DriverEx> collection = sender as UniqueObservableCollection<DriverEx>;
                    if (collection.Count > 0)
                    {
                        DriverEx element = collection[collection.Count - 1];
                        name = element.DriverName;
                    }
                }
                catch (Exception ex)
                {
                    name = ex.Message;
                }
#endif
            }
            else if (sender.GetType().Equals(typeof(UniqueObservableCollection<KeyValuePair<int, TimeSpan>>)))
            {
                UpdateLapTime = true;
#if DEBUG
                try
                {
                    UniqueObservableCollection<KeyValuePair<int, TimeSpan>> collection = sender as UniqueObservableCollection<KeyValuePair<int, TimeSpan>>;
                    if (collection.Count > 0)
                    {
                        KeyValuePair<int, TimeSpan> element = collection[collection.Count - 1];
                        name = element.Key.ToString() + " " + ((TimeSpan)(element.Value)).ToString();
                    }
                }
                catch (Exception ex)
                {
                    name = ex.Message;
                }
#endif
            }
#if DEBUG
            Console.WriteLine("CollectionChanged " + e.Action + " " + name);
#endif
        }
    }
}
