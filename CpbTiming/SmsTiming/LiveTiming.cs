using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace CpbTiming.SmsTiming
{
    public class LiveTiming : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public enum HeatStateEnum
        {
            HeatNotStarted = 0,
            HeatRunning = 1,
            HeatPauzed = 2,
            HeatStopped = 3,
            HeatFinished = 4,
            NextHeat = 5
        };

        public static Dictionary<string, int> HeatStateDict { get; } = new Dictionary<string, int>
        {
            {"Heat not started.", 0 },
            {"Heat running.", 1 },
            {"Heat pauzed.", 2 },
            {"Heat stopped.", 3 },
            {"Heat finished.", 4 },
            {"Next heat.", 5 }
        };

        public static Dictionary<string, int> EndConditionDict { get; } = new Dictionary<string, int>
        {
            {"The heat needs to be finished manual.", 0 },
            {"The heat finishes after X time.", 1 },
            {"The heat finishes after X laps.", 2 },
            {"The heat finished after X time or X laps. Depends on wich one is first.", 3 }
        };

        public static Dictionary<string, int> RaceModeDict { get; } = new Dictionary<string, int>
        {
            {"Most laps wins.", 0},
            {"The best laptime is the winner.", 1},
            {"The best average time is the winner.", 2}
        };

        public static Dictionary<string, int> ClockStartedDict { get; } = new Dictionary<string, int>
        {
            {"Clock not started.", 0},
            {"Clock Started.", 1}
        };

        protected string _HeatName;
        protected int? _HeatState;
        protected int? _EndCondition;
        protected int? _RaceMode;
        protected UniqueObservableCollection<Driver> _Drivers = new UniqueObservableCollection<Driver>();
        protected TimeSpan _TimeLeft;
        protected bool? _ClockStarted;
        protected int? _RemainingLaps;
        protected int? _ActualHeatStart;

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
                RaisePropertyChanged("HeatName");
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
                RaisePropertyChanged("HeatState");
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
                RaisePropertyChanged("EndCondition");
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
                RaisePropertyChanged("RaceMode");
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
                _Drivers.Sort();
                RaisePropertyChanged("Drivers");
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

        public TimeSpan TimeLeft
        {
            get
            {
                return _TimeLeft;
            }
            set
            {
                _TimeLeft = value;
                RaisePropertyChanged("TimeLeft");
            }
        }

        /// <summary>
        /// "CS" = ClockStarted
        /// </summary>
        [JsonProperty(PropertyName = "CS")]
        public bool? ClockStarted
        {
            get
            {
                return _ClockStarted;
            }
            set
            {
                _ClockStarted = value;
                RaisePropertyChanged("CounterStarted");
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
                RaisePropertyChanged("RemainingLaps");
            }
        }

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
                RaisePropertyChanged("ActualHeatStart");
            }
        }

        private void ResetHeatName()
        {
            _HeatName = null;
        }

        private void ResetHeatState()
        {
            _HeatState = null;
        }

        private void ResetEndCondition()
        {
            _EndCondition = null;
        }

        private void ResetRaceMode()
        {
            _RaceMode = null;
        }

        private void ResetDrivers()
        {
            _Drivers.Clear();
        }

        private void ResetTimeLeft()
        {
            _TimeLeft = TimeSpan.Zero;
        }

        private void ResetClockStarted()
        {
            _ClockStarted = null;
        }

        private void ResetRemainingLaps()
        {
            _RemainingLaps = null;
        }

        private void ResetActualHeatStart()
        {
            _ActualHeatStart = null;
        }

        public void Reset()
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(GetType()))
            {
                if (prop.CanResetValue(this))
                {
                    prop.ResetValue(this);
                    RaisePropertyChanged(prop.Name);
                }
            }
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}