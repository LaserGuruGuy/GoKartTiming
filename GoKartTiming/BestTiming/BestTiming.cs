using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Newtonsoft.Json;

namespace GoKartTiming.BestTiming
{
    public class BestTiming : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        private ObservableCollection<ScoreGroup> _scoregroupcollection = new ObservableCollection<ScoreGroup>();
        private ObservableCollection<ParameterGroup> _parametergroupcollection = new ObservableCollection<ParameterGroup>();
        private ObservableCollection<RecordGroup[]> _recordgroupcollection = new ObservableCollection<RecordGroup[]>();

        public ScoreGroup[] scoregroup
        {
            set
            {
                foreach (var score in value)
                {
                    scoregroupcollection.Add(score);
                }
            }
        }

        public ParameterGroup parametergroup { set { parametergroupcollection.Add(value); } }

        public RecordGroup[] recordgroup { set { recordgroupcollection.Add(value); } }

        [JsonIgnore]
        public ObservableCollection<ScoreGroup> scoregroupcollection
        {
            get
            {
                return _scoregroupcollection;
            }
            private set
            {
                _scoregroupcollection = value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        public ObservableCollection<ParameterGroup> parametergroupcollection
        {
            get
            {
                return _parametergroupcollection;
            }
            private set
            {
                _parametergroupcollection = value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        public ObservableCollection<RecordGroup[]> recordgroupcollection
        {
            get
            {
                return _recordgroupcollection;
            }
            private set
            {
                _recordgroupcollection = value;
                RaisePropertyChanged();
            }
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
