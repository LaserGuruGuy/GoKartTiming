﻿using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace CpbTiming.SmsTiming
{
    public class Driver : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private int? _DriverID;
        private int? _WebMemberID;
        private string _DriverName;
        private int? _KartNumber;
        private int? _Position;
        private int? _Laps;
        private UniqueKeyedCollection _LapTime = new UniqueKeyedCollection();
        private TimeSpan _LastLapTime;
        private TimeSpan _AvarageLapTime;
        private TimeSpan _BestLapTime;
        private string _GapTime;
        private bool? _LastPassing;
        private int? _LastRecord;

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
                RaisePropertyChanged("DriverID");
            }
        }

        /// <summary>
        /// "M" = WebMemberID
        /// </summary>
        [JsonProperty(PropertyName = "W")]
        public int? WebMemberID
        {
            get
            {
                return _WebMemberID;
            }
            set
            {
                _WebMemberID = value;
                RaisePropertyChanged("WebMemberID");
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
                RaisePropertyChanged("DriverName");
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
                KartNumber = int.Parse(value);
            }
        }

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
                RaisePropertyChanged("KartNumber");
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
                _Position = value;
                RaisePropertyChanged("Position");
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
                RaisePropertyChanged("Laps");
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
                LastLapTime = TimeSpan.FromMilliseconds(value);
            }
        }

        [JsonIgnore]
        public TimeSpan LastLapTime
        {
            get
            {
                return _LastLapTime;
            }
            set
            {
                _LastLapTime = value;
                RaisePropertyChanged("LastLapTime");
                _LapTime.Add(new KeyedTimeSpan { Key = (int)_Laps, Value = _LastLapTime });
                RaisePropertyChanged("LapTime");
            }
        }

        [JsonIgnore]
        public UniqueKeyedCollection LapTime
        {
            get
            {
                return _LapTime;
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
                AvarageLapTime = TimeSpan.FromMilliseconds(value);
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
                RaisePropertyChanged("AvarageLapTime");
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
                BestLapTime = TimeSpan.FromMilliseconds(value);
            }
        }

        [JsonIgnore]
        public TimeSpan BestLapTime
        {
            get
            {
                return _BestLapTime;
            }
            set
            {
                _BestLapTime = value;
                RaisePropertyChanged("BestLapTime");
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
                RaisePropertyChanged("GapTime");
            }
        }

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
                RaisePropertyChanged("LastPassing");
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
                RaisePropertyChanged("LastRecord");
            }
        }

        private void ResetDriverID()
        {
            _DriverID = null;
        }

        private void ResetWebMemberID()
        {
            _WebMemberID = null;
        }

        private void ResetDriverName()
        {
            _DriverName = null;
        }

        private void ResetKartNumber()
        {
            _KartNumber = null;
        }

        private void ResetPosition()
        {
            _Position = null;
        }

        private void ResetLaps()
        {
            _Laps = null;
        }

        private void ResetLapTime()
        {
            _LapTime = new UniqueKeyedCollection();
        }

        private void ResetLastLapTime()
        {
            _LastLapTime = TimeSpan.Zero;
        }

        private void ResetAvarageLapTime()
        {
            _AvarageLapTime = TimeSpan.Zero;
        }

        private void ResetBestLapTime()
        {
            _BestLapTime = TimeSpan.Zero;
        }

        private void ResetGapTime()
        {
            _GapTime = null;
        }

        private void ResetLastPassing()
        {
            _LastPassing = null;
        }

        private void ResetLastRecord()
        {
            _LastRecord = null;
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