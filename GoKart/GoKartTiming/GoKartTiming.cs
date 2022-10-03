using System.ComponentModel;
using System.Collections.Specialized;
using System.IO;

namespace GoKart
{
    public partial class GoKartTiming : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        private object _lock = new object();

        private string _RaceStatus;

        public string RaceStatus
        {
            get
            {
                lock (_lock)
                {
                    return _RaceStatus;
                }
            }
            set
            {
                lock (_lock)
                {
                    _RaceStatus = value;
                }
                RaisePropertyChanged();
            }
        }

        public GoKartTiming()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
        }

        ~GoKartTiming()
        {
        }

        public void AddTextBook(object data)
        {
            System.Windows.Data.BindingOperations.EnableCollectionSynchronization(LiveTimingCollection, _lock);
            if (data.GetType().Equals(typeof(string)))
            {
                string TextBook = data as string;
                AddLiveTiming(ExtractTextBookFromPdf(TextBook));
            }
            System.Windows.Data.BindingOperations.DisableCollectionSynchronization(_LiveTimingCollection);
        }

        public void AddJson(object data)
        {
            System.Windows.Data.BindingOperations.EnableCollectionSynchronization(LiveTimingCollection, _lock);
            if (data.GetType().Equals(typeof(string)))
            {
                string FileName = data as string;
                foreach (string Serialized in File.ReadAllLines(FileName))
                {
                    AddLiveTiming(Serialized);
                }
            }
            System.Windows.Data.BindingOperations.DisableCollectionSynchronization(_LiveTimingCollection);
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