using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Newtonsoft.Json;

namespace GoKartTiming.LiveTiming
{
    public class Positions
    {
        public int position { get; set; } // "1"
        public string date { get; set; } // "2022-09-13T21:19:19.447"
        public string participant { get; set; } // "blalba"
        public string score { get; set; } // "38.288"
    }

    public class LiveTiming : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        public static Dictionary<string, int> HeatStateDict { get; } = new Dictionary<string, int>
        {
            {"Heat not started", 0 },
            {"Heat running", 1 },
            {"Heat pauzed", 2 },
            {"Heat stopped", 3 },
            {"Heat finished", 4 },
            {"Next heat", 5 }
        };

        public static Dictionary<string, int> EndConditionDict { get; } = new Dictionary<string, int>
        {
            {"The heat needs to be finished manual", 0 },
            {"The heat finishes after X time", 1 },
            {"The heat finishes after X laps", 2 },
            {"The heat finished after X time or X laps depending on wich one is first", 3 }
        };

        public static Dictionary<string, int> RaceModeDict { get; } = new Dictionary<string, int>
        {
            {"Most laps wins", 0},
            {"The best laptime is the winner", 1},
            {"The best average time is the winner", 2}
        };

        public static Dictionary<string, int> ClockStartedDict { get; } = new Dictionary<string, int>
        {
            {"Clock not started", 0},
            {"Clock Started", 1}
        };

        protected int? _ActualHeatStart;
        protected int? _ClockEnabled;
        protected int? _ClockStarted;
        protected UniqueObservableCollection<Driver> _Drivers;
        protected int? _EndMode;
        protected TimeSpan _TimeLeft;
        protected string _HeatName;
        protected int? _EndCondition;
        protected int? _RaceMode;
        protected int? _RemainingLaps;
        protected int? _HeatState;

        public UniqueObservableCollection<Positions> records { get; set; }

        /// <summary>
        /// "T" = ActualHeatStart
        /// </summary>
        [JsonProperty(PropertyName = "T")]
        public int? ActualHeatStart
        {
            get
            {
                return _ActualHeatStart;
            }
            set
            {
                _ActualHeatStart = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// "CE" = ClockEnabled
        /// </summary>
        [JsonProperty(PropertyName = "CE")]
        public int? ClockEnabled
        {
            get
            {
                return _ClockEnabled;
            }
            set
            {
                _ClockEnabled = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// "CS" = ClockStarted
        /// </summary>
        [JsonProperty(PropertyName = "CS")]
        public int? ClockStarted
        {
            get
            {
                return _ClockStarted;
            }
            set
            {
                _ClockStarted = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// "D" = Drivers [array]
        /// </summary>
        [JsonProperty(PropertyName = "D")]
        public UniqueObservableCollection<Driver> Drivers
        {
            get
            {
                return _Drivers;
            }
            set
            {
                _Drivers = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// "EM" = EndMode
        /// </summary>
        [JsonProperty(PropertyName = "EM")]
        public int? EndMode
        {
            get
            {
                return _EndMode;
            }
            set
            {
                _EndMode = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// "C" = Counter (in milliseconds)
        /// </summary>
        [JsonProperty(PropertyName = "C")]
        public int Counter
        {
            set
            {
                TimeLeft = TimeSpan.FromMilliseconds(value);
            }
        }

        /// <summary>
        /// TimeLeft => viewmodel
        /// </summary>
        [JsonIgnore]
        public TimeSpan TimeLeft
        {
            get
            {
                return _TimeLeft;
            }
            set
            {
                _TimeLeft = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// "N" = HeatName
        /// </summary>
        [JsonProperty(PropertyName = "N")]
        public string HeatName
        {
            get
            {
                return _HeatName;
            }
            set
            {
                _HeatName = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// "E" = EndCondition
        /// </summary>
        [JsonProperty(PropertyName = "E")]
        public int? EndCondition
        {
            get
            {
                return _EndCondition;
            }
            set
            {
                _EndCondition = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// "R" = RaceMode
        /// </summary>
        [JsonProperty(PropertyName = "R")]
        public int? RaceMode
        {
            get
            {
                return _RaceMode;
            }
            set
            {
                _RaceMode = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// "L" = RemainingLaps
        /// </summary>
        [JsonProperty(PropertyName = "L")]
        public int? RemainingLaps
        {
            get
            {
                return _RemainingLaps;
            }
            set
            {
                _RemainingLaps = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// "S" = HeatState 
        /// </summary>
        [JsonProperty(PropertyName = "S")]
        public int? HeatState
        {
            get
            {
                return _HeatState;
            }
            set
            {
                _HeatState = value;
                RaisePropertyChanged();
            }
        }

        private void ResetActualHeatStart()
        {
            _ActualHeatStart = null;
        }

        private void ResetClockEnabled()
        {
            _ClockEnabled = null;
        }

        private void ResetClockStarted()
        {
            _ClockStarted = null;
        }

        private void ResetDrivers()
        {
            _Drivers.Clear();
        }

        private void ResetEndMode()
        {
            _EndMode = null;
        }

        private void ResetTimeLeft()
        {
            _TimeLeft = TimeSpan.Zero;
        }

        private void ResetHeatName()
        {
            _HeatName = null;
        }

        private void ResetEndCondition()
        {
            _EndCondition = null;
        }

        private void ResetRaceMode()
        {
            _RaceMode = null;
        }

        private void ResetRemainingLaps()
        {
            _RemainingLaps = null;
        }

        private void ResetHeatState()
        {
            _HeatState = null;
        }

        public void Reset()
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(GetType()))
            {
                if (prop.CanResetValue(this))
                {
                    prop.ResetValue(this);
                    RaisePropertyChanged(prop.Name);
                    Console.WriteLine(prop.Name);
                }
            }
        }

        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        protected void RaiseCollectionChanged(NotifyCollectionChangedAction action, object changedItem)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItem));
        }
    }
}