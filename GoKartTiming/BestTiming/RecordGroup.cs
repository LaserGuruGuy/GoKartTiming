using System;
using System.ComponentModel;
using System.Globalization;

namespace GoKartTiming.BestTiming
{
    public class RecordGroup : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #region backing
        private int _Position;
        private DateTime _Date;
        private string _Participant;
        private TimeSpan _Score;
        #endregion

        #region translaters
        public string position
        {
            set
            {
                Position = int.Parse(value);
            }
        }
        public string date
        {
            set
            {
                Date = DateTime.Parse(value.Replace('T', ' '));
            }
        }
        public string participant
        {
            set
            {
                Participant = value;
            }
        }
        public string score
        {
            set
            {
                try
                {
                    Score = TimeSpan.ParseExact(value, @"s\.fff", CultureInfo.InvariantCulture);
                }
                catch
                {
                    try
                    {
                        Score = TimeSpan.ParseExact(value, @"m\:ss\.fff", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                    }
                }
            }
        }
        #endregion

        #region properties
        public int Position
        {
            get
            {
                return _Position;
            }
            private set
            {
                _Position = value;
                RaisePropertyChanged();
            }
        }

        public DateTime Date
        {
            get
            {
                return _Date;
            }
            private set
            {
                _Date = value;
                RaisePropertyChanged();
            }
        }

        public string Participant
        {
            get
            {
                return _Participant;
            }
            private set
            {
                _Participant = value;
                RaisePropertyChanged();
            }
        }

        public TimeSpan Score
        {
            get
            {
                return _Score;
            }
            private set
            {
                _Score = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
