using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.Specialized;
using System.IO;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;
using GoKartTiming.LiveTiming;

namespace GoKart
{
    public partial class CpbTiming : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        private object _lock = new object();

        private UniqueObservableCollection<LiveTimingEx> _LiveTimingCollection = new UniqueObservableCollection<LiveTimingEx>();

        public UniqueObservableCollection<LiveTimingEx> LiveTimingCollection
        {
            get
            {
                lock (_lock)
                {
                    return _LiveTimingCollection;
                }
            }
            set
            {
                lock (_lock)
                {
                    _LiveTimingCollection = value;
                }
                RaisePropertyChanged("LiveTimingCollection");
            }
        }

        public CpbTiming()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
        }

        ~CpbTiming()
        {
        }

        public void AddTextBook(object data)
        {
            System.Windows.Data.BindingOperations.EnableCollectionSynchronization(LiveTimingCollection, _lock);
            if (data.GetType().Equals(typeof(string)))
            {
                string TextBook = data as string;
                Add(ExtractTextBookFromPdf(TextBook));
            }
            System.Windows.Data.BindingOperations.DisableCollectionSynchronization(_LiveTimingCollection);
        }

        public void AddJson(object data)
        {
            System.Windows.Data.BindingOperations.EnableCollectionSynchronization(LiveTimingCollection, _lock);
            if (data.GetType().Equals(typeof(string)))
            {
                string FileName = data as string;
                foreach (string Serialized in File.ReadAllLines(FileName))
                {
                    Add(Serialized);
                }
            }
            System.Windows.Data.BindingOperations.DisableCollectionSynchronization(_LiveTimingCollection);
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
                            lock (_lock)
                            {
                                try
                                {
                                    JsonConvert.PopulateObject(Serialized, LiveTimingCollection[i], new JsonSerializerSettings
                                    {
                                        ObjectCreationHandling = ObjectCreationHandling.Reuse,
                                        ContractResolver = new InterfaceContractResolver(typeof(LiveTimingEx))
                                    });
                                }
                                catch { }
                                finally
                                {
                                    //RaiseCollectionChanged(NotifyCollectionChangedAction.Add, LiveTimingCollection);
                                    LiveTimingCollection[i].Drivers.Sort();
                                }
                            }
                            return;
                        }
                    }
                }
                lock (_lock)
                {
                    try
                    {
                        LiveTimingCollection.Add(JsonConvert.DeserializeObject<LiveTimingEx>(Serialized, new JsonSerializerSettings
                        {
                            ObjectCreationHandling = ObjectCreationHandling.Reuse,
                            ContractResolver = new InterfaceContractResolver(typeof(LiveTimingEx))
                        }));
                    }
                    catch { }
                    finally
                    {
                        if (LiveTimingCollection.Count > 0)
                        {
                            LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers.Sort();
                            //RaiseCollectionChanged(NotifyCollectionChangedAction.Add, LiveTimingCollection);
                            LiveTimingCollection[LiveTimingCollection.Count - 1].PropertyChanged += PropertyChanged;
                            LiveTimingCollection[LiveTimingCollection.Count - 1].CollectionChanged += CollectionChanged;
                            if (LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers != null)
                            {
                                for (var i = 0; i < LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers.Count; i++)
                                {
                                    LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers[i].PropertyChanged += PropertyChanged;
                                    LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers[i].CollectionChanged += CollectionChanged;
                                    LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers[i].LapTime.CollectionChanged += CollectionChanged;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                lock (_lock)
                {
                    try
                    {
                        LiveTimingCollection.Add(JsonConvert.DeserializeObject<LiveTimingEx>(Serialized, new JsonSerializerSettings
                        {
                            ObjectCreationHandling = ObjectCreationHandling.Reuse,
                            ContractResolver = new InterfaceContractResolver(typeof(LiveTimingEx))
                        }));
                    }
                    catch { }
                    finally
                    {
                        if (LiveTimingCollection.Count > 0)
                        {
                            LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers.Sort();
                            //RaiseCollectionChanged(NotifyCollectionChangedAction.Add, LiveTimingCollection);
                            LiveTimingCollection[LiveTimingCollection.Count - 1].PropertyChanged += PropertyChanged;
                            LiveTimingCollection[LiveTimingCollection.Count - 1].CollectionChanged += CollectionChanged;
                            if (LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers != null)
                            {
                                for (var i = 0; i < LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers.Count; i++)
                                {
                                    LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers[i].PropertyChanged += PropertyChanged;
                                    LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers[i].CollectionChanged += CollectionChanged;
                                    LiveTimingCollection[LiveTimingCollection.Count - 1].Drivers[i].LapTime.CollectionChanged += CollectionChanged;
                                }
                            }
                        }
                    }
                }
            }
        }

        private System.Windows.Input.ICommand _DeleteLiveTiming;

        public System.Windows.Input.ICommand DeleteLiveTiming => _DeleteLiveTiming ?? (_DeleteLiveTiming = new RelayCommand<LiveTimingEx>(DeleteLiveTimingCommand));

        public void DeleteLiveTimingCommand(LiveTimingEx LiveTiming)
        {
            lock (_lock)
            {
                LiveTimingCollection.Remove(LiveTiming);
            }
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