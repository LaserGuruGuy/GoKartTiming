using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace GoKartTiming.LiveTiming
{
    public class LiveTiming : ILiveTiming, INotifyPropertyChanged, INotifyCollectionChanged
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
        protected TimeSpan? _TimeStart;
        protected string _HeatName;
        protected int? _EndCondition;
        protected int? _RaceMode;
        protected int? _RemainingLaps;
        protected int? _HeatState;

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

        public int Counter
        {
            set
            {
                TimeLeft = TimeSpan.FromMilliseconds(value);
            }
            get
            {
                return (int)TimeLeft.TotalMilliseconds;
            }
        }

        public TimeSpan? TimeStart
        {
            get
            {
                return _TimeStart;
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
                RaisePropertyChanged();
            }
        }

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

        public int? EndCondition
        {
            get
            {
                return _EndCondition;
            }
            set
            {
                // try to catch the total heat time
                if (_TimeStart.HasValue.Equals(false) || _EndCondition.GetValueOrDefault().Equals(value).Equals(false))
                {
                    switch (value)
                    {
                        case 0:
                            // "The heat needs to be finished manual" => time counting up
                            _TimeStart = TimeSpan.Zero;
                            break;
                        case 2:
                            // "The heat finishes after X laps" => time counting up
                            _TimeStart = TimeSpan.Zero;
                            break;
                        case 1:
                            //"The heat finishes after X time" => time counting down
                            _TimeStart = TimeLeft;
                            break;
                        case 3:
                            //"The heat finished after X time or X laps depending on wich one is first" => time counting down
                            _TimeStart = TimeLeft;
                            break;
                    }
                }
                _EndCondition = value;
                RaisePropertyChanged();
            }
        }

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

        private void ResetTimeStart()
        {
            _TimeStart = null;
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