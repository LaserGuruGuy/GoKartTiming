using Newtonsoft.Json;

namespace GoKartTiming.LiveTiming
{
    public interface IDriver
    {
        /// <summary>
        /// "LP" = LastPassing
        ///      0 = not the last passing
        ///      1 = last passing
        /// </summary>
        [JsonProperty(PropertyName = "LP")]
        public bool? LastPassing { get; set; }

        /// <summary>
        /// "A" = AvarageLapTimeMS
        /// </summary>
        [JsonProperty(PropertyName = "A")]
        public int AvarageLapTimeTotalMilliseconds {set; }

        /// <summary>
        /// "B" = BestLapTimeMS
        /// </summary>
        [JsonProperty(PropertyName = "B")]
        public int BestLapTimeTotalMilliseconds { set; }

        /// <summary>
        /// "K" = KartNumber
        /// </summary>
        [JsonProperty(PropertyName = "K")]
        string KartNumberString { set; }

        /// <summary>
        /// "G" = GapTime
        /// Gap in laps
        /// </summary>
        [JsonProperty(PropertyName = "G")]
        string GapTime { get; set; }

        /// <summary>
        /// "D" = DriverID
        /// </summary>
        [JsonProperty(PropertyName = "D")]
        int? DriverID { get; set; }

        /// <summary>
        /// "L" = Laps
        /// </summary>
        [JsonProperty(PropertyName = "L")]
        int? Laps { get; set; }

        /// <summary>
        /// "T" = LastLapTimeMS
        /// </summary>
        [JsonProperty(PropertyName = "T")]
        int LastLapTimeTotalMilliseconds { set; }

        /// <summary>
        /// "R" = LastRecord
        /// </summary>
        [JsonProperty(PropertyName = "R")]
        int? LastRecord { get; set; }

        /// <summary>
        /// "N" = DriverName
        /// </summary>
        [JsonProperty(PropertyName = "N")]
        string DriverName { get; set; }

        /// <summary>
        /// "P" = Position
        /// </summary>
        [JsonProperty(PropertyName = "P")]
        int? Position { get; set; }

        /// <summary>
        /// "M" = MemberID
        /// </summary>
        [JsonProperty(PropertyName = "M")]
        int? MemberID { get; set; }
    }
}