using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.Specialized;
using System.IO;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;
using CpbTiming.SmsTiming;

namespace GoKart
{
    public partial class CpbTiming : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

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

        public void AddTextBook(object data)
        {
            if (data.GetType().Equals(typeof(string)))
            {
                string TextBook = data as string;
                Add(ExtractTextBookFromPdf(TextBook));
            }
        }

        public void AddJson(object data)
        {
            if (data.GetType().Equals(typeof(string)))
            {
                string FileName = data as string;
                foreach (string Serialized in File.ReadAllLines(FileName))
                {
                    Add(Serialized);
                }
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
                    if (!string.IsNullOrEmpty(LiveTimingCollection[i].HeatName))
                    {
                        if (Serialized.Contains(LiveTimingCollection[i].HeatName))
                        {
                            JsonConvert.PopulateObject(Serialized, LiveTimingCollection[i], new JsonSerializerSettings
                            {
                                ObjectCreationHandling = ObjectCreationHandling.Reuse,
                                ContractResolver = new InterfaceContractResolver(typeof(LiveTimingEx))
                            });
                            //LiveTimingCollection[i].Drivers.Sort();
                            return;
                        }
                    }
                }
                LiveTimingCollection.Add(JsonConvert.DeserializeObject<LiveTimingEx>(Serialized));
                LiveTimingCollection[LiveTimingCollection.Count - 1].PropertyChanged += PropertyChanged;
                LiveTimingCollection[LiveTimingCollection.Count - 1].CollectionChanged += CollectionChanged;
                for (var i = 0; i < LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers.Count; i++)
                {
                    LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers[i].PropertyChanged += PropertyChanged;
                    LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers[i].CollectionChanged += CollectionChanged;
                    LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers[i].LapTime.CollectionChanged += CollectionChanged;
                }
            }
            else
            {
                LiveTimingCollection.Add(JsonConvert.DeserializeObject<LiveTimingEx>(Serialized));
                LiveTimingCollection[LiveTimingCollection.Count - 1].PropertyChanged += PropertyChanged;
                LiveTimingCollection[LiveTimingCollection.Count - 1].CollectionChanged += CollectionChanged;
                for (var i = 0; i < LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers.Count; i++)
                {
                    LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers[i].PropertyChanged += PropertyChanged;
                    LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers[i].CollectionChanged += CollectionChanged;
                    LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers[i].LapTime.CollectionChanged += CollectionChanged;
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaiseCollectionChanged(NotifyCollectionChangedAction action, object changedItem)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItem));
        }
    }
}