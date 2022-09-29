using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;

namespace GoKartTiming.BestTiming
{
    public class BestTiming : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        private ObservableCollection<ScoreGroup> _scoregroupcollection = new ObservableCollection<ScoreGroup>();
        private ObservableCollection<RecordGroup> _recordgroupcollection = new ObservableCollection<RecordGroup>();

        public string key { get; set; }

        public string id { get; set; }

        public string name { get; set; }

        public string resourceId { get; set; }

        public string resourceName { get; set; }

        public int resourceKind { get; set; }

        [JsonProperty(PropertyName = "scoregroups")]
        public ScoreGroup[] scoregroup
        {
            set
            {
                scoregroupcollection.Clear();
                scoregroupcollection.Add(new ScoreGroup() { id = string.Empty, name="All", scoreGroupId = string.Empty, scoreGroupName = "All" });

                foreach (var score in value)
                {
                    scoregroupcollection.Add(score);
                }
            }
        }

        [JsonProperty(PropertyName = "records")]
        public RecordGroup[] recordgroup
        {
            set
            {
                _recordgroupcollection.Clear();
                foreach (var record in value)
                {
                    _recordgroupcollection.Add(record);
                }
            }
        }

        [JsonIgnore]
        public ObservableCollection<ScoreGroup> scoregroupcollection
        {
            get
            {
                return _scoregroupcollection;
            }
            set
            {
                _scoregroupcollection = value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        public ObservableCollection<RecordGroup> recordgroupcollection
        {
            get
            {
                return _recordgroupcollection;
            }
            set
            {
                _recordgroupcollection = value;
                RaisePropertyChanged();
            }
        }

        public static Dictionary<string, DateTime> DateTimeDict { get; private set; } = new Dictionary<string, DateTime>();

        public BestTiming()
        {
            ResetDateTimeDict();
        }

        private void Resetscoregroupcollection()
        {
            _scoregroupcollection.Clear();
        }

        private void Resetrecordgroupcollection()
        {
            _recordgroupcollection.Clear();
        }

        private void ResetDateTimeDict()
        {
            var now = DateTime.Now;

            int diff = (int)now.DayOfWeek - (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var week = now.AddDays(-(diff < 0 ? 7 + diff : diff));

            DateTimeDict.Add("Forever", new DateTime(now.Year, 1, 1).AddYears(-20));
            DateTimeDict.Add("This Year", new DateTime(now.Year, 1, 1));
            DateTimeDict.Add("This Month", new DateTime(now.Year, now.Month, 1));
            DateTimeDict.Add("This Week", new DateTime(week.Year, week.Month, week.Day));
            DateTimeDict.Add("Today", new DateTime(now.Year, now.Month, now.Day));
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
