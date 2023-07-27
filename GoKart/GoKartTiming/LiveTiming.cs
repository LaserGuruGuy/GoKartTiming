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
        private enum HeatStateEnum
        {
            HeatNotStarted = 0,
            HeatRunning = 1,
            HeatPauzed = 2,
            HeatStopped = 3,
            HeatFinished = 4,
            NextHeat = 5
        }

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

                                // try to catch the elapsed heat time
                                foreach (var Driver in LiveTimingCollection[i].Drivers)
                                {
                                    //TODO: hoe de laatste positie tot het einde van de race door te trekken?
                                    if (LiveTimingCollection[i].TimeStart.HasValue.Equals(true))
                                    {
                                        switch (LiveTimingCollection[i].EndCondition)
                                        {
                                            case 0:
                                                // "The heat needs to be finished manual" => time counting up
                                                Driver.TimeElapsed = LiveTimingCollection[i].TimeLeft;
                                                break;
                                            case 1:
                                                //"The heat finishes after X time" => time counting down
                                                switch (LiveTimingCollection[i].HeatState.GetValueOrDefault())
                                                {
                                                    case ((int)HeatStateEnum.HeatNotStarted):
                                                    case ((int)HeatStateEnum.HeatPauzed):
                                                    case ((int)HeatStateEnum.HeatStopped):
                                                        break;
                                                    case ((int)HeatStateEnum.HeatRunning):
                                                        Driver.TimeElapsed = LiveTimingCollection[i].TimeStart.GetValueOrDefault() - LiveTimingCollection[i].TimeLeft;
                                                        break;
                                                    case ((int)HeatStateEnum.HeatFinished):
                                                        Driver.TimeElapsed = LiveTimingCollection[i].TimeStart.GetValueOrDefault();
                                                        break;
                                                    case ((int)HeatStateEnum.NextHeat):
                                                        Driver.TimeElapsed = TimeSpan.Zero;
                                                        break;
                                                }
                                                break;
                                            case 2:
                                                // "The heat finishes after X laps" => time counting up
                                                Driver.TimeElapsed = LiveTimingCollection[i].TimeLeft;
                                                break;
                                            case 3:
                                                //"The heat finished after X time or X laps depending on wich one is first" => time counting down
                                                Driver.TimeElapsed = LiveTimingCollection[i].TimeStart.GetValueOrDefault() - LiveTimingCollection[i].TimeLeft;
                                                break;
                                        }
                                    }
                                }

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
                            LiveTimingCollection[i].Drivers[j].LapTimePosition.CollectionChanged += CollectionChanged;
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