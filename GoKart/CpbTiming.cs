using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using CpbTiming.SmsTiming;
using Newtonsoft.Json;
using static CpbTiming.SmsTiming.LiveTiming;

namespace GoKart
{
    public class CpbTiming : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        
        private UniqueObservableCollection<LiveTimingEx> _LiveTimingCollection = new UniqueObservableCollection<LiveTimingEx>();

        private LiveTimingEx _LiveTiming = new LiveTimingEx();

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

        public LiveTimingEx LiveTiming
        { 
            get
            {
                return _LiveTiming;
            }
            set
            {
                _LiveTiming = value;
                RaisePropertyChanged("LiveTiming");
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
            JsonConvert.PopulateObject(Serialized, LiveTiming, new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Reuse,
                ContractResolver = new InterfaceContractResolver(typeof(LiveTimingEx))
            });
            
            switch (LiveTiming.HeatState)
            {
                case (int)HeatStateEnum.HeatNotStarted:
                    break;
                case (int)HeatStateEnum.HeatRunning:
                    LiveTimingCollection.Add(LiveTiming);
                    break;
                case (int)HeatStateEnum.HeatPauzed:
                    break;
                case (int)HeatStateEnum.HeatStopped:
                    break;
                case (int)HeatStateEnum.HeatFinished:
                    LiveTiming.Reset();
                    break;
                case (int)HeatStateEnum.NextHeat:
                    break;
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