using Newtonsoft.Json;

namespace GoKartTiming.LiveTiming
{
    public interface ILiveTiming
    {
        /// <summary>
        /// "T" = ActualHeatStart
        /// </summary>
        [JsonProperty(PropertyName = "T")]
        int? ActualHeatStart { get; set; }

        /// <summary>
        /// "CE" = ClockEnabled
        /// </summary>
        [JsonProperty(PropertyName = "CE")]
        int? ClockEnabled { get; set; }

        /// <summary>
        /// "CS" = ClockStarted
        /// </summary>
        [JsonProperty(PropertyName = "CS")]
        int? ClockStarted { get; set; }

        /// <summary>
        /// "D" = Drivers [array]
        /// </summary>
        [JsonProperty(PropertyName = "D")]
        UniqueObservableCollection<Driver> Drivers { get; set; }

        /// <summary>
        /// "EM" = EndMode
        /// </summary>
        [JsonProperty(PropertyName = "EM")]
        public int? EndMode { get; set; }

        /// <summary>
        /// "C" = Counter (in milliseconds)
        /// </summary>
        [JsonProperty(PropertyName = "C")]
        int Counter { get; set; }

        /// <summary>
        /// "N" = HeatName
        /// </summary>
        [JsonProperty(PropertyName = "N")]
        string HeatName { get; set; }

        /// <summary>
        /// "E" = EndCondition
        /// </summary>
        [JsonProperty(PropertyName = "E")]
        int? EndCondition { get; set; }

        /// <summary>
        /// "R" = RaceMode
        /// </summary>
        [JsonProperty(PropertyName = "R")]
        int? RaceMode { get; set; }

        /// <summary>
        /// "L" = RemainingLaps
        /// </summary>
        [JsonProperty(PropertyName = "L")]
        int? RemainingLaps { get; set; }

        /// <summary>
        /// "S" = HeatState 
        /// </summary>
        [JsonProperty(PropertyName = "S")]
        int? HeatState { get; set; }
    }
}
