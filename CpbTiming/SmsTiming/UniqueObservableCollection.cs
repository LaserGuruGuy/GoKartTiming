using System.Collections.ObjectModel;

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
                        //System.Console.WriteLine("[" + DestinationProperty.Name + "]=" + DestinationProperty.GetValue(Source, new object[] { }) + "=>" + SourceProperty.GetValue(Source, new object[] { }));
                        DestinationProperty.SetValue(Destination, SourceProperty.GetValue(Source, new object[] { }), new object[] { });
                        OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(DestinationProperty.Name));
                        break;
                    }
                }
            }
        }

        protected override void InsertItem(int Index, T Item)
        {
            if (GetType() == typeof(UniqueObservableCollection<LiveTimingEx>) && Item.GetType() == typeof(LiveTimingEx))
            {
                for (var idx = 0; idx < Items.Count; idx++)
                {
                    LiveTimingEx Destination = Items[idx] as LiveTimingEx;
                    LiveTimingEx Source = Item as LiveTimingEx;
                    if (Destination.HeatName.Equals(Source.HeatName))
                    {
                        AssignItem(Destination, Source);
                        return;
                    }
                }
                Items.Add(Item);
                OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Add, Items[Items.Count-1]));
            }
            else if (GetType() == typeof(UniqueObservableCollection<DriverEx>) && (Item.GetType() == typeof(DriverEx)))
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
                OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Add, Items[Items.Count - 1]));
            }
        }
    }
}