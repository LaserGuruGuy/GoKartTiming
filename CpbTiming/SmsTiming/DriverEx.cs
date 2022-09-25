using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;

namespace CpbTiming.SmsTiming
{
    public class DriverEx : Driver, INotifyPropertyChanged, INotifyCollectionChanged
    {
        private float? _AverageSpeed;

        [JsonIgnore]
        public float? AverageSpeed
        {
            get
            {
                return _AverageSpeed;
            }
            set
            {
                _AverageSpeed = value;
                RaisePropertyChanged();
            }
        }

        private void ResetAverageSpeed()
        {
            _AverageSpeed = null;
        }

        public bool Parse(string Line, ref string HeatName, ref DateTime DateTime)
        {
            //Positie
            if (Line.Equals("Positie"))
            {
                Position = null;
                return true;
            }
            //1. EIK#3 - 56
            else if (Position == null && DriverName == null && KartNumber == null)
            {
                string Prefix = string.Empty;
                string Suffix = string.Empty;
                if (Line.LastIndexOf('-') > -1) Suffix = Line.Substring(Line.LastIndexOf('-') + 1).TrimStart();
                if (Line.IndexOf('.') > -1) Prefix = Line.Remove(Line.IndexOf('.'));
                DriverName = Line.Replace(Prefix + ".", string.Empty).Replace(Suffix, string.Empty).Trim();
                if (DriverName.LastIndexOf('-') > -1) DriverName = DriverName.Remove(DriverName.LastIndexOf('-'), 1).TrimEnd();
                int _Position;
                if (int.TryParse(Prefix, out _Position))
                {
                    Position = _Position;
                    int _KartNumber;
                    if (int.TryParse(Suffix, out _KartNumber))
                    {
                        KartNumber = _KartNumber;
                        return true;
                    }
                }
            }
            else
            {
                string[] Items = Line.Split(':');
                //Name:
                if (Items[0].Equals("Name"))
                {
                    //this.DriverName = Items[1].Trim();
                    return true;
                }
                //Heat: Karten - 2 Uurs Race
                else if (Items[0].Equals("Heat"))
                {
                    HeatName = Items[1].Trim();
                    return true;
                }
                //Datum: 10-8-2022 18:00:00
                else if (Items[0].Equals("Datum"))
                {
                    string DateTimeString = Items[1].Trim();
                    for (int i = 2; i < Items.Length; i++) DateTimeString += ":" + Items[i];

                    if (DateTime.TryParseExact(DateTimeString, "d-M-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AllowInnerWhite, out DateTime) == false)
                    {
                        if (DateTime.TryParseExact(DateTimeString, "dd-M-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AllowInnerWhite, out DateTime) == false)
                        {
                            if (DateTime.TryParseExact(DateTimeString, "d-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AllowInnerWhite, out DateTime) == false)
                            {
                                if (DateTime.TryParseExact(DateTimeString, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AllowInnerWhite, out DateTime) == false)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    return true;
                }
                //1
                else
                {
                    int _Position;
                    if (int.TryParse(Items[0], out _Position))
                    {
                        if (Position == _Position)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool Parse(string Line)
        {
            string[] Items = Line.Split(' ');
            if (Items.Length == 2)
            {
                int _Laps;
                if (int.TryParse(Items[0], out _Laps))
                {
                    Laps = _Laps;
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
                    LastLapTime = RondeTijd;
                    return true;
                }
            }
            return false;
        }
    }
}