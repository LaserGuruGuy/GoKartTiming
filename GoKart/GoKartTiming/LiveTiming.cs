using System.Collections.Generic;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;
using GoKartTiming.LiveTiming;
using System;
using System.IO;

namespace GoKart
{
    public partial class GoKartTiming
    {
        private string LocalApplicationDataFolder { get; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\GoKart\\";

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

        public void AddLiveTiming(List<string> Book)
        {
            LiveTimingEx LiveTiming = new LiveTimingEx();
            foreach (string Page in Book)
            {
                LiveTiming.Parse(Page);
            }
            LiveTimingCollection.Add(LiveTiming);
        }

        public void AddLiveTiming(string Serialized)
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
                                    LiveTimingCollection[i].Drivers.Sort();

                                    string FileName = LocalApplicationDataFolder + LiveTimingCollection[i].DateTime.ToString("yyyyMMdd") + " " + LiveTimingCollection[i].HeatName + ".json";
                                    File.AppendAllText(FileName, Serialized + "\n");
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
                            ObjectCreationHandling = ObjectCreationHandling.Auto,
                            ContractResolver = new InterfaceContractResolver(typeof(LiveTimingEx))
                        }));
                    }
                    catch { }
                    finally
                    {
                        if (LiveTimingCollection.Count > 0)
                        {
                            var i = LiveTimingCollection.Count - 1;

                            LiveTimingCollection[i].Drivers.Sort();

                            LiveTimingCollection[i].PropertyChanged += PropertyChanged;
                            LiveTimingCollection[i].CollectionChanged += CollectionChanged;

                            if (LiveTimingCollection[i].Drivers != null)
                            {
                                for (var j = 0; j < LiveTimingCollection[i].Drivers.Count; j++)
                                {
                                    LiveTimingCollection[i].Drivers[j].PropertyChanged += PropertyChanged;
                                    LiveTimingCollection[i].Drivers[j].CollectionChanged += CollectionChanged;
                                    LiveTimingCollection[i].Drivers[j].LapTime.CollectionChanged += CollectionChanged;
                                }
                            }

                            string FileName = LocalApplicationDataFolder + LiveTimingCollection[i].DateTime.ToString("yyyyMMdd") + " " + LiveTimingCollection[i].HeatName + ".json";
                            File.AppendAllText(FileName, Serialized + "\n");
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
                            ObjectCreationHandling = ObjectCreationHandling.Auto,
                            ContractResolver = new InterfaceContractResolver(typeof(LiveTimingEx))
                        }));
                    }
                    catch { }
                    finally
                    {
                        if (LiveTimingCollection.Count > 0)
                        {
                            var i = LiveTimingCollection.Count - 1;

                            LiveTimingCollection[i].Drivers.Sort();

                            LiveTimingCollection[i].PropertyChanged += PropertyChanged;
                            LiveTimingCollection[i].CollectionChanged += CollectionChanged;

                            if (LiveTimingCollection[i].Drivers != null)
                            {
                                for (var j = 0; j < LiveTimingCollection[i].Drivers.Count; j++)
                                {
                                    LiveTimingCollection[i].Drivers[j].PropertyChanged += PropertyChanged;
                                    LiveTimingCollection[i].Drivers[j].CollectionChanged += CollectionChanged;
                                    LiveTimingCollection[i].Drivers[j].LapTime.CollectionChanged += CollectionChanged;
                                }
                            }

                            string FileName = LocalApplicationDataFolder + LiveTimingCollection[i].DateTime.ToString("yyyyMMdd") + " " + LiveTimingCollection[i].HeatName + ".json";
                            File.AppendAllText(FileName, Serialized + "\n");
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
    }
}