using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using CpbTiming.SmsTiming;
using Newtonsoft.Json;
using static CpbTiming.SmsTiming.LiveTiming;
using System;

namespace GoKart
{
    public class CpbTiming : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        
        private UniqueObservableCollection<LiveTimingEx> _LiveTimingCollection = new UniqueObservableCollection<LiveTimingEx>();

        public UniqueObservableCollection<LiveTimingEx> LiveTimingCollection
        {
            get
            {
                return _LiveTimingCollection;
            }
            set
            {
                _LiveTimingCollection = value;
                RaisePropertyChanged("LiveTimingCollection");
            }
        }

        public void Add(List<string> Book)
        {
            LiveTimingEx LiveTiming = new LiveTimingEx();
            foreach (string Page in Book)
            {
                LiveTiming.Parse(Page);
            }
            LiveTimingCollection.Add(LiveTiming);
        }

        public void Add(string Serialized)
        {
            if (LiveTimingCollection.Count > 0)
            {
                for (var i = 0; i < LiveTimingCollection.Count; i++)
                {
                    if (Serialized.Contains(LiveTimingCollection[i].HeatName))
                    {
                        JsonConvert.PopulateObject(Serialized, LiveTimingCollection[i], new JsonSerializerSettings
                        {
                            ObjectCreationHandling = ObjectCreationHandling.Reuse,
                            ContractResolver = new InterfaceContractResolver(typeof(LiveTimingEx))
                        });
                        LiveTimingCollection[i].Drivers.Sort();
                        return;
                    }
                }
                LiveTimingCollection.Add(JsonConvert.DeserializeObject<LiveTimingEx>(Serialized));
                LiveTimingCollection[LiveTimingCollection.Count - 1].PropertyChanged += PropertyChanged;
                for (var i = 0; i < LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers.Count; i++)
                {
                    LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers[i].PropertyChanged += PropertyChanged;
                }
            }
            else
            {
                LiveTimingCollection.Add(JsonConvert.DeserializeObject<LiveTimingEx>(Serialized));
                LiveTimingCollection[LiveTimingCollection.Count - 1].PropertyChanged += PropertyChanged;
                for (var i = 0; i < LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers.Count; i++)
                {
                    LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers[i].PropertyChanged += PropertyChanged;
                }
            }
        }

        private System.Windows.Input.ICommand _DeleteLiveTiming;

        public System.Windows.Input.ICommand DeleteLiveTiming => _DeleteLiveTiming ?? (_DeleteLiveTiming = new RelayCommand<LiveTimingEx>(DeleteLiveTimingCommand));

        public void DeleteLiveTimingCommand(LiveTimingEx LiveTiming)
        {
            LiveTimingCollection.Remove(LiveTiming);
            RaisePropertyChanged("LiveTimingCollection");
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}