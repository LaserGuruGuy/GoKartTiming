using System.ComponentModel;

namespace GoKartTiming.BestTiming
{
    public class ScoreGroup : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private string _id;
        private string _name;
        private string _scoreGroupId;
        private string _scoreGroupName;

        public string id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                RaisePropertyChanged();
            }
        }

        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public string scoreGroupId
        {
            get
            {
                return _scoreGroupId;
            }
            set
            {
                _scoreGroupId = value;
                RaisePropertyChanged();
            }
        }

        public string scoreGroupName
        {
            get
            {
                return _scoreGroupName;
            }
            set
            {
                _scoreGroupName = value;
                RaisePropertyChanged();
            }
        }

        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
