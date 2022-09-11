using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CpbTiming.SmsTiming
{
    public class UniqueObservableCollection<T> : ObservableCollection<T>
    {
        public void AssignItem(object Destination, object Source)
        {
            var DestinationProperties = Destination.GetType().GetProperties();
            foreach (var SourceProperty in Source.GetType().GetProperties())
            {
                foreach (var DestinationProperty in DestinationProperties)
                {
                    if (DestinationProperty.Name == SourceProperty.Name && SourceProperty.CanRead && DestinationProperty.CanWrite && DestinationProperty.PropertyType.IsAssignableFrom(SourceProperty.PropertyType))
                    {
                        DestinationProperty.SetValue(Destination, SourceProperty.GetValue(Source, new object[] { }), new object[] { });
                    }
                }
            }
        }

        protected override void InsertItem(int Index, T Item)
        {
            if (GetType() == typeof(UniqueObservableCollection<LiveTimingEx>) && Item?.GetType() == typeof(LiveTimingEx))
            {
                for (var idx = 0; idx < Items.Count; idx++)
                {
                    LiveTimingEx Destination = Items[idx] as LiveTimingEx;
                    LiveTimingEx Source = Item as LiveTimingEx;
                    if (!string.IsNullOrEmpty(Destination.HeatName) && !string.IsNullOrEmpty(Source.HeatName))
                    {
                        if (Destination.HeatName.Equals(Source.HeatName))
                        {
                            AssignItem(Destination, Source);
                            return;
                        }
                    }
                }
                Items.Add(Item);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Items));
            }
            else if (GetType() == typeof(UniqueObservableCollection<DriverEx>) && (Item?.GetType() == typeof(DriverEx)))
            {
                for (var idx = 0; idx < Items.Count; idx++)
                {
                    DriverEx Destination = Items[idx] as DriverEx;
                    DriverEx Source = Item as DriverEx;
                    if (Destination.DriverName.Equals(Source.DriverName))
                    {
                        AssignItem(Destination, Source);
                        return;
                    }
                }
                Items.Add(Item);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Items));
            }
            else if (GetType() == typeof(UniqueObservableCollection<KeyValuePair<int, TimeSpan>>) && (Item?.GetType() == typeof(KeyValuePair<int, TimeSpan>)))
            {
                if (!Items.Contains(Item))
                {
                    Items.Add(Item);
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Items));
                }
            }
            else
            {
                Items.Add(Item);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Items));
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            //if (SynchronizationContext.Current != null)
            {
                // We are in the creator thread, call the base implementation directly
                try
                {
                    base.OnCollectionChanged(e);
                }
                catch (SystemException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            //if (SynchronizationContext.Current != null)
            {
                // We are in the creator thread, call the base implementation directly
                try
                {
                    base.OnPropertyChanged(e);
                }
                catch (SystemException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}