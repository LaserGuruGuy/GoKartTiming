using Newtonsoft.Json;

namespace GoKartTiming.LiveTiming
{
    public interface ILiveTimingEx : ILiveTiming
    {
        /// <summary>
        /// "D" = Drivers [array]
        /// </summary>
        [JsonProperty(PropertyName = "D")]
        new UniqueObservableCollection<DriverEx> Drivers { get; set; }
    }
}
