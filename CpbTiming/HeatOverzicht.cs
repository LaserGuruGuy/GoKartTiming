﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace CpbTiming
{
    public class HeatOverzicht : Dictionary<string, TimeSpan>
    {
        public bool Parse(string Line)
        {
            string[] Items = Line.Split(' ');
            int Positie;
            if (int.TryParse(Items[0], out Positie))
            {
                TimeSpan RondeTijd;
                if (TimeSpan.TryParseExact(Items[Items.Length - 1], @"ss\.fff", CultureInfo.InvariantCulture, out RondeTijd) == false)
                {
                    if (TimeSpan.TryParseExact(Items[Items.Length - 1], @"m\:ss\.fff", CultureInfo.InvariantCulture, out RondeTijd) == false)
                    {
                        if (TimeSpan.TryParseExact(Items[Items.Length - 1], @"mm\:ss\.fff", CultureInfo.InvariantCulture, out RondeTijd) == false)
                        {
                            return false;
                        }
                    }
                }
                this.Add(Line.Substring(Line.IndexOf(' ')).Remove(Line.LastIndexOf(' ')), RondeTijd);
                return true;
            }
            return false;
        }
    }
}