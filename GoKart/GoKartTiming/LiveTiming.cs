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

        public void AddLiveTiming(string Serialized, bool LogToFile = false)
        {
            lock (_lock)
            {
                if (!AssignItem(Serialized, LogToFile))
                {
                    AddItem(Serialized, LogToFile);
                }
            }
        }

        private bool AssignItem(string Serialized, bool LogToFile)
        {
            if (LiveTimingCollection.Count > 0)
            {
                for (var i = 0; i < LiveTimingCollection.Count; i++)
                {
                    if (!string.IsNullOrEmpty(LiveTimingCollection[i].HeatName))
                    {
                        if (Serialized.Contains(LiveTimingCollection[i].HeatName))
                        {
                            try
                            {
                                JsonConvert.PopulateObject(Serialized, LiveTimingCollection[i], new JsonSerializerSettings
                                {
                                    ObjectCreationHandling = ObjectCreationHandling.Reuse,
                                    ContractResolver = new InterfaceContractResolver(typeof(ILiveTimingEx), typeof(IDriverEx))
                                });
                            }
                            catch { }
                            finally
                            {
                                LiveTimingCollection[i].Drivers.Sort();

                                if (LogToFile)
                                {
                                    string FileName = LocalApplicationDataFolder + LiveTimingCollection[i].DateTime.ToString("yyyyMMdd") + " " + LiveTimingCollection[i].HeatName + ".json";
                                    File.AppendAllText(FileName, Serialized + "\n");
                                }
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void AddItem(string Serialized, bool LogToFile)
        {
            try
            {
                LiveTimingCollection.Add(JsonConvert.DeserializeObject<LiveTimingEx>(Serialized, new JsonSerializerSettings
                {
                    ObjectCreationHandling = ObjectCreationHandling.Auto,
                    ContractResolver = new InterfaceContractResolver(typeof(ILiveTimingEx), typeof(IDriverEx))
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

                    if (LogToFile)
                    {
                        string FileName = LocalApplicationDataFolder + LiveTimingCollection[i].DateTime.ToString("yyyyMMdd") + " " + LiveTimingCollection[i].HeatName + ".json";
                        File.AppendAllText(FileName, Serialized + "\n");
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