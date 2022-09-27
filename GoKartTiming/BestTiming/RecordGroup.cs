using System;
using System.Globalization;

namespace GoKartTiming.BestTiming
{
    public class RecordGroup
    {
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
                Date = DateTime.Parse(value.Replace('T',' '));
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
        public int Position { get; private set; }
        public DateTime Date { get; private set; }
        public string Participant { get; private set; }
        public TimeSpan Score { get; private set; }
        #endregion
    }
}
