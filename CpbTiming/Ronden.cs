using System;
using System.Collections.Generic;
using System.Globalization;

namespace CpbTiming
{
    public class Ronden : Dictionary<int, TimeSpan>
    {
        public bool Parse(string Line)
        {
            string[] Items = Line.Split(' ');
            if (Items.Length == 2)
            {
                int RondeNummer;
                if (int.TryParse(Items[0], out RondeNummer))
                {
                    TimeSpan RondeTijd;
                    if (TimeSpan.TryParseExact(Items[1], @"s\.fff", CultureInfo.InvariantCulture, out RondeTijd) == false)
                    {
                        if (TimeSpan.TryParseExact(Items[1], @"ss\.fff", CultureInfo.InvariantCulture, out RondeTijd) == false)
                        {
                            if (TimeSpan.TryParseExact(Items[1], @"m\:ss\.fff", CultureInfo.InvariantCulture, out RondeTijd) == false)
                            {
                                if (TimeSpan.TryParseExact(Items[1], @"mm\:ss\.fff", CultureInfo.InvariantCulture, out RondeTijd) == false)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    this.Add(RondeNummer, RondeTijd);
                    return true;
                }
            }
            return false;
        }
    }
}