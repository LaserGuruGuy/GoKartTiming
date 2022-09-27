using System.ComponentModel;
using System.Collections.Specialized;
using System.IO;

namespace GoKart
{
    public partial class CpbTiming : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        private object _lock = new object();

        public CpbTiming()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
        }

        ~CpbTiming()
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

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaiseCollectionChanged(NotifyCollectionChangedAction action, object changedItem)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItem));
        }
    }
}