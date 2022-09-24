using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;

namespace CpbTiming.SmsTiming
{
    public class UniqueObservableCollection<T> : ObservableCollection<T>, INotifyCollectionChanged
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
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Item));
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
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Item));
            }
            else if (GetType() == typeof(UniqueObservableCollection<KeyValuePair<int, TimeSpan>>) && (Item?.GetType() == typeof(KeyValuePair<int, TimeSpan>)))
            {
                if (!Items.Contains(Item))
                {
                    Items.Add(Item);
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Item));
                }
            }
            else
            {
                Items.Add(Item);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Item));
            }
        }

        public new event NotifyCollectionChangedEventHandler CollectionChanged;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            var notifyCollectionChangedEventHandler = CollectionChanged;

            if (notifyCollectionChangedEventHandler == null)
            {
                return;
            }

            foreach (NotifyCollectionChangedEventHandler handler in notifyCollectionChangedEventHandler.GetInvocationList())
            {
                var dispatcherObject = handler.Target as DispatcherObject;

                if (dispatcherObject != null && !dispatcherObject.CheckAccess())
                {
                    try
                    {
                        dispatcherObject.VerifyAccess();
                        try
                        {
                            dispatcherObject.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, handler, this, args);
                        }
                        catch (ArgumentException ex)
                        {

                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        try
                        {
                            dispatcherObject.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, handler, this, args);
                        }
                        catch
                        {

                        }
                        // "The calling thread cannot access this object because a different thread owns it."
                    }
                }
                else
                {
                    try
                    {
                        handler(this, args); // note : this does not execute handler in target thread's context
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}