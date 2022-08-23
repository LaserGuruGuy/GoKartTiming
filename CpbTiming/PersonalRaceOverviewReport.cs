using System;
using System.Collections.Generic;

namespace CpbTiming
{
    public class PersonalRaceOverviewReport
    {
        public List<RaceOverviewReport> RaceOverviewReport { get; set; } = new List<RaceOverviewReport>();
        public string RaceName { get; set; }
        public DateTime RaceDateTime { get; set; }

        public PersonalRaceOverviewReport(string Title, DateTime DateTime, List<string> Book = null)
        {
            RaceDateTime = DateTime;

            if (!string.IsNullOrEmpty(Title))
            {
                RaceName = Title;
            }

            if (Book != null)
            {
                Parse(Book);
            }
        }

        public void Parse(List<string> Book)
        {
            foreach (var Page in Book)
            {
                RaceOverviewReport.Add(new RaceOverviewReport(Page));
            }
        }
    }
}