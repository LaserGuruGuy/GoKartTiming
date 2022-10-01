using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Newtonsoft.Json;

namespace GoKartTiming.LiveTiming
{
    public class Driver : INotifyPropertyChanged, INotifyCollectionChanged
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
        private int? _LastRecord;
        private string _DriverName;
        private int? _Position;
        private int? _ImprovedPosition;
        private int? _MemberID;

        private string _LastRecordString;

        /// <summary>
        /// "LP" = LastPassing
        ///      0 = not the last passing
        ///      1 = last passing
        /// </summary>
        [JsonProperty(PropertyName = "LP")]
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

        /// <summary>
        /// "A" = AvarageLapTimeMS
        /// </summary>
        [JsonProperty(PropertyName = "A")]
        public int AvarageLapTimeTotalMilliseconds
        {
            set
            {
                _AvarageLapTime = TimeSpan.FromMilliseconds(value);
            }
        }

        [JsonIgnore]
        public TimeSpan AvarageLapTime
        {
            get
            {
                return _AvarageLapTime;
            }
            set
            {
                _AvarageLapTime = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// "B" = BestLapTimeMS
        /// </summary>
        [JsonProperty(PropertyName = "B")]
        public int BestLapTimeTotalMilliseconds
        {
            set
            {
                _BestLapTime = TimeSpan.FromMilliseconds(value);
            }
        }

        /// <summary>
        /// BestLaptime => viewmodel
        /// </summary>
        [JsonIgnore]
        public TimeSpan BestLapTime
        {
            get
            {
                return _BestLapTime;
            }
            set
            {
                ImprovedBestLapTime = value != null && _BestLapTime != null ? value < _BestLapTime && _BestLapTime != TimeSpan.Zero ? true : false : false;
                _BestLapTime = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// ImprovedBestLapTime => viewmodel
        /// Fires the DataTrigger StoryBoard
        /// </summary>
        [JsonIgnore]
        public bool? ImprovedBestLapTime
        {
            get
            {
                return _ImprovedBestLapTime;
            }
            set
            {
                if (value != null)
                {
                    _ImprovedBestLapTime = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// "K" = KartNumber
        /// </summary>
        [JsonProperty(PropertyName = "K")]
        public string KartNumberString
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        _KartNumber = int.Parse(value);
                    }
                    catch
                    {

                    }
                }
            }
        }

        /// <summary>
        /// KartNumber => viewmodel
        /// </summary>
        [JsonIgnore]
        public int? KartNumber
        {
            get
            {
                return _KartNumber;
            }
            set
            {
                _KartNumber = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// "G" = GapTime
        /// Gap in laps
        /// </summary>
        [JsonProperty(PropertyName = "G")]
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

        /// <summary>
        /// "D" = DriverID
        /// </summary>
        [JsonProperty(PropertyName = "D")]
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

        /// <summary>
        /// "L" = Laps
        /// </summary>
        [JsonProperty(PropertyName = "L")]
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

        /// <summary>
        /// "T" = LastLapTimeMS
        /// </summary>
        [JsonProperty(PropertyName = "T")]
        public int LastLapTimeTotalMilliseconds
        {
            set
            {
                _LastLapTime = TimeSpan.FromMilliseconds(value);
            }
        }

        /// <summary>
        /// LastLapTime => viewmodel
        /// </summary>
        [JsonIgnore]
        public TimeSpan LastLapTime
        {
            get
            {
                return _LastLapTime;
            }
            set
            {
                ImprovedLastLapTime = value != null && _LastLapTime != null ? value < _LastLapTime && _LastLapTime != TimeSpan.Zero ? -1 : value > _LastLapTime && _LastLapTime != TimeSpan.Zero ? +1 : 0 : 0;
                _LastLapTime = value;
                RaisePropertyChanged();
                _LapTime.Add(new KeyValuePair<int, TimeSpan>((int)_Laps, _LastLapTime));
            }
        }

        /// <summary>
        /// ImprovedLastLapTime => viewmodel
        /// Fires the DataTrigger StoryBoard
        /// {-1, "Green"} {+1, "Red"}
        /// </summary>
        [JsonIgnore]
        public int? ImprovedLastLapTime
        {
            get
            {
                return _ImprovedLastLapTime;
            }
            set
            {
                if (value != null)
                {
                    _ImprovedLastLapTime = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// LapTime List => viewmodel
        /// </summary>
        [JsonIgnore]
        public UniqueObservableCollection<KeyValuePair<int, TimeSpan>> LapTime
        {
            get
            {
                return _LapTime;
            }
        }

        /// <summary>
        /// "R" = LastRecord
        /// </summary>
        [JsonProperty(PropertyName = "R")]
        public int? LastRecord
        {
            get
            {
                return _LastRecord;
            }
            set
            {
                _LastRecord = value;
                RaisePropertyChanged();
                if (LastRecordDict.TryGetValue((int)value, out _LastRecordString))
                {
                    RaisePropertyChanged("LastRecordString");
                }
            }
        }

        public string LastRecordString
        {
            get
            {
                return _LastRecordString;
            }
            set
            {
                _LastRecordString = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        /// "N" = DriverName
        /// </summary>
        [JsonProperty(PropertyName = "N")]
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

        /// <summary>
        /// "P" = Position
        /// </summary>
        [JsonProperty(PropertyName = "P")]
        public int? Position
        {
            get
            {
                return _Position;
            }
            set
            {
                bool bUpdate = _Position != null && value != null && _Position != value ? true : false;
                if (bUpdate) ImprovedPosition = _Position != null && value != null ? value < _Position ? -1 : value > _Position ? +1 : 0 : 0;
                _Position = value;
                if (bUpdate) RaisePropertyChanged();
            }
        }

        /// <summary>
        /// ImprovedPosition => viewmodel
        /// Fires the DataTrigger StoryBoard
        /// {-1, "Green"} {+1, "Red"}
        /// </summary>
        [JsonIgnore]
        public int? ImprovedPosition
        {
            get
            {
                return _ImprovedPosition;
            }
            set
            {
                if (value != null)
                {
                    _ImprovedPosition = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// "M" = MemberID
        /// </summary>
        [JsonProperty(PropertyName = "M")]
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
            _ImprovedBestLapTime = false;
        }

        private void ResetLapTime()
        {
            _LapTime = new UniqueObservableCollection<KeyValuePair<int, TimeSpan>>();
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

        private void ResetImprovedPosition()
        {
            _ImprovedPosition = 0;
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