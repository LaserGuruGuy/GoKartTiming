using GoKartTiming.BestTiming;
using Newtonsoft.Json;

namespace GoKart
{
    public partial class CpbTiming
    {
        private BestTiming _BestTimingCollection = new BestTiming();

        public BestTiming BestTimingCollection
        {
            get
            {
                lock (_lock)
                {
                    return _BestTimingCollection;
                }
            }
            set
            {
                lock (_lock)
                {
                    _BestTimingCollection = value;
                }
                RaisePropertyChanged("BestTimingCollection");
            }
        }

        public void AddBestTiming(string Serialized)
        {
            lock (_lock)
            {
                try
                {
                    JsonConvert.PopulateObject(Serialized, BestTimingCollection, new JsonSerializerSettings
                    {
                        ObjectCreationHandling = ObjectCreationHandling.Reuse,
                        ContractResolver = new InterfaceContractResolver(typeof(BestTiming))
                    });
                }
                catch { }
                finally
                {
                }
            }
        }
    }
}