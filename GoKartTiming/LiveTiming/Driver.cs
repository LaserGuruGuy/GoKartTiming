using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace GoKartTiming.LiveTiming
{
    public class Driver : IDriver, INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        private static Dictionary<int, string> LastRecordDict { get; } = new Dictionary<int, string>
        {
            {0, "Day"},
            {1, "Week"},
            {2, "Month"},
            {3, "Year"},
            {4, "Ever"},
            {5, ""}
        };

        private bool? _LastPassing;
        private TimeSpan _AvarageLapTime;
        private TimeSpan _BestLapTime;
        private bool? _ImprovedBestLapTime;
        private int? _KartNumber;
        private string _GapTime;
        private int? _DriverID;
        private int? _Laps;
        private TimeSpan _LastLapTime;
        private int? _ImprovedLastLapTime;
        private UniqueObservableCollection<KeyValuePair<int, TimeSpan>> _LapTime = new UniqueObservableCollection<KeyValuePair<int, TimeSpan>>();
        private UniqueObservableCollection<KeyValuePair<int, TimeSpan>> _LapTimePosition = new UniqueObservableCollection<KeyValuePair<int, TimeSpan>>();
        private int? _LastRecord;
        private string _LastRecordString;
        private string _DriverName;
        private int? _Position;
        private int? _DeltaPosition;
        private int? _MemberID;

        public bool? LastPassing
        {
            get
            {
                return _LastPassing;
            }
            set
            {
                _LastPassing = value;
                RaisePropertyChanged();
            }
        }

        public int AvarageLapTimeTotalMilliseconds
        {
            set
            {
                AvarageLapTime = TimeSpan.FromMilliseconds(value);
            }
        }

        public TimeSpan AvarageLapTime
        {
            get
            {
                return _AvarageLapTime;
            }
            protected set
            {
                _AvarageLapTime = value;
                RaisePropertyChanged();
            }
        }

        public int BestLapTimeTotalMilliseconds
        {
            set
            {
                BestLapTime = TimeSpan.FromMilliseconds(value);
            }
        }

        public TimeSpan BestLapTime
        {
            get
            {
                return _BestLapTime;
            }
            protected set
            {
                if (value < _BestLapTime)
                {
                    ImprovedBestLapTime = true;
                }
                else
                {
                    ImprovedBestLapTime = false;
                }
                _BestLapTime = value;
                RaisePropertyChanged();
            }
        }

        public bool? ImprovedBestLapTime
        {
            get
            {
                return _ImprovedBestLapTime;
            }
            private set
            {
                _ImprovedBestLapTime = value;
                RaisePropertyChanged();
            }
        }

        public string KartNumberString
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        KartNumber = int.Parse(value);
                    }
                    catch
                    {

                    }
                }
            }
        }

        public int? KartNumber
        {
            get
            {
                return _KartNumber;
            }
            protected set
            {
                _KartNumber = value;
                RaisePropertyChanged();
            }
        }

        public string GapTime
        {
            get
            {
                return _GapTime;
            }
            set
            {
                _GapTime = value;
                RaisePropertyChanged();
            }
        }

        public int? DriverID
        {
            get
            {
                return _DriverID;
            }
            set
            {
                _DriverID = value;
                RaisePropertyChanged();
            }
        }

        public int? Laps
        {
            get
            {
                return _Laps;
            }
            set
            {
                _Laps = value;
                RaisePropertyChanged();
            }
        }

        public int LastLapTimeTotalMilliseconds
        {
            set
            {
                LastLapTime = TimeSpan.FromMilliseconds(value);
            }
        }

        public TimeSpan LastLapTime
        {
            get
            {
                return _LastLapTime;
            }
            set
            {
                if (value != TimeSpan.Zero)
                {
                    if (value < _LastLapTime && _LastLapTime != TimeSpan.Zero)
                    {
                        _ImprovedLastLapTime = -1;
                    }
                    else if (value > _LastLapTime && _LastLapTime != TimeSpan.Zero)
                    {
                        _ImprovedLastLapTime = +1;
                    }
                    else
                    {
                        _ImprovedLastLapTime = 0;
                    }
                    RaisePropertyChanged("ImprovedLastLapTime");
                }
                _LastLapTime = value;
                RaisePropertyChanged();
                _LapTime.Add(new KeyValuePair<int, TimeSpan>((int)_Laps, value));
            }
        }

        public int? ImprovedLastLapTime
        {
            get
            {
                return _ImprovedLastLapTime;
            }
        }

        public UniqueObservableCollection<KeyValuePair<int, TimeSpan>> LapTime
        {
            get
            {
                return _LapTime;
            }
        }

        public int? LastRecord
        {
            get
            {
                return _LastRecord;
            }
            set
            {
                if (value < 5)
                {
                    if (LastRecordDict.TryGetValue((int)value, out _LastRecordString))
                    {
                        LastRecordString = _LastRecordString;
                    }
                }
                _LastRecord = value;
                RaisePropertyChanged();
            }
        }

        public string LastRecordString
        {
            get
            {
                return _LastRecordString;
            }
            protected set
            {
                _LastRecordString = value;
                RaisePropertyChanged();
            }
        }

        public string DriverName
        {
            get
            {
                return _DriverName;
            }
            set
            {
                _DriverName = value;
                RaisePropertyChanged();
            }
        }

        public int? Position
        {
            get
            {
                return _Position;
            }
            set
            {
                if (value != null)
                {
                    if (_Position != null)
                    {
                        if (value < _Position)
                        {
                            DeltaPosition = -30;
                        }
                        else if (value > _Position)
                        {
                            DeltaPosition = +30;
                        }
                        else
                        {
                            if (DeltaPosition > 0)
                            {
                                DeltaPosition--;
                            }
                            else if (DeltaPosition < 0)
                            {
                                DeltaPosition++;
                            }
                        }
                    }
                    _Position = value;
                    RaisePropertyChanged();

                }
            }
        }

        public TimeSpan TimeElapsed
        {
            set
            {
                //if (_LapTimePosition.LastOrDefault().Key.Equals(Position).Equals(false))
                {
                    _LapTimePosition.Add(new KeyValuePair<int, TimeSpan>(_Position.GetValueOrDefault(), value));
                }
            }
        }

        public UniqueObservableCollection<KeyValuePair<int, TimeSpan>> LapTimePosition
        {
            get
            {
                return _LapTimePosition;
            }
        }

        public int? DeltaPosition
        {
            get
            {
                return _DeltaPosition;
            }
            protected set
            {
                _DeltaPosition = value;
                RaisePropertyChanged();
            }
        }

        public int? MemberID
        {
            get
            {
                return _MemberID;
            }
            set
            {
                _MemberID = value;
                RaisePropertyChanged();
            }
        }

        private void ResetLastPassing()
        {
            _LastPassing = null;
        }

        private void ResetAvarageLapTime()
        {
            _AvarageLapTime = TimeSpan.Zero;
        }

        private void ResetBestLapTime()
        {
            _BestLapTime = TimeSpan.Zero;
        }

        private void ResetImprovedBestLapTime()
        {
            _ImprovedBestLapTime = null;
        }

        private void ResetLapTime()
        {
            _LapTime = new UniqueObservableCollection<KeyValuePair<int, TimeSpan>>();
        }

        private void ResetLapTimePosition()
        {
            _LapTimePosition = new UniqueObservableCollection<KeyValuePair<int, TimeSpan>>();
        }

        private void ResetKartNumber()
        {
            _KartNumber = null;
        }

        private void ResetGapTime()
        {
            _GapTime = null;
        }

        private void ResetDriverID()
        {
            _DriverID = null;
        }

        private void ResetLaps()
        {
            _Laps = null;
        }

        private void ResetLastLapTime()
        {
            _LastLapTime = TimeSpan.Zero;
        }

        private void ResetImprovedLastLapTime()
        {
            _ImprovedLastLapTime = 0;
        }

        private void ResetLastRecord()
        {
            _LastRecord = null;
        }

        private void ResetDriverName()
        {
            _DriverName = null;
        }

        private void ResetPosition()
        {
            _Position = null;
        }

        private void ResetDeltaPosition()
        {
            _DeltaPosition = 0;
        }

        private void ResetMemberID()
        {
            _MemberID = null;
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