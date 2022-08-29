//using System;
//using System.Globalization;
//using CpbTiming.SmsTiming;

//namespace GoKart
//{
//    public static class CpbTimingExtensions
//    {
//        public static LiveTiming Parse(this LiveTiming Me, string Page)
//        {
//            if (Me.GetType() == typeof(LiveTiming))
//            {
//                LiveTiming LiveTiming = Me as LiveTiming;

//                if (LiveTiming.Drivers == null)
//                {
//                    LiveTiming.Drivers = new UniqueObservableCollection<Driver>();
//                }

//                string HeatName = string.Empty;
//                DateTime DateTime = new DateTime();
//                Driver Driver = new Driver();

//                string[] Lines = Page.Split('\n');
//                for (int line = 0; line < Lines.Length; line++)
//                {
//                    if (Lines[line].Equals("Persoonlijk Overzicht"))
//                    {
//                        while (Driver.Parse(Lines[line + 1], ref HeatName, ref DateTime)) line++;
//                        LiveTiming.HeatName = HeatName;
//                        LiveTiming.DateTime = DateTime;
//                    }
//                    else if (Lines[line].Equals("Uw race geschiedenis (Beste Ronde)"))
//                    {
//                        ;
//                    }
//                    else if (Lines[line].Equals("UW BESTE RONDE Gemiddelde snelheid Gemiddelde rondetijd"))
//                    {
//                        line++;
//                        string[] Chunk = Lines[line].Split(' ');

//                        for (var idx = 0; idx < Chunk.Length; idx++)
//                        {
//                            switch (idx)
//                            {
//                                // UW BESTE RONDE
//                                case 0:
//                                    TimeSpan BestLapTime = new TimeSpan();
//                                    if (TimeSpan.TryParseExact(Chunk[idx], @"ss\.fff", CultureInfo.InvariantCulture, out BestLapTime) == false)
//                                    {
//                                        if (TimeSpan.TryParseExact(Chunk[idx], @"m\:ss\.fff", CultureInfo.InvariantCulture, out BestLapTime) == false)
//                                        {
//                                            if (TimeSpan.TryParseExact(Chunk[idx], @"h\:mm\:ss\.fff", CultureInfo.InvariantCulture, out BestLapTime) == false)
//                                            {
//                                                break;
//                                            }
//                                        }
//                                    }
//                                    Driver.BestLapTime = BestLapTime;
//                                    break;
//                                // Gemiddelde snelheid
//                                case 1:
//                                    Driver.AverageSpeed = float.Parse(Chunk[idx], new CultureInfo(CultureInfo.InvariantCulture.Name)
//                                    {
//                                        NumberFormat = { NumberDecimalSeparator = "," }
//                                    });
//                                    break;
//                                // Gemiddelde rondetijd
//                                case 2:
//                                    TimeSpan AvarageLapTime = new TimeSpan();
//                                    if (TimeSpan.TryParseExact(Chunk[idx], @"s\.fff", CultureInfo.InvariantCulture, out AvarageLapTime) == false)
//                                    {
//                                        if (TimeSpan.TryParseExact(Chunk[idx], @"ss\.fff", CultureInfo.InvariantCulture, out AvarageLapTime) == false)
//                                        {
//                                            if (TimeSpan.TryParseExact(Chunk[idx], @"m\:ss\.fff", CultureInfo.InvariantCulture, out AvarageLapTime) == false)
//                                            {
//                                                if (TimeSpan.TryParseExact(Chunk[idx], @"mm\:ss\.fff", CultureInfo.InvariantCulture, out AvarageLapTime) == false)
//                                                {
//                                                    break;
//                                                }
//                                            }
//                                        }
//                                    }
//                                    Driver.AvarageLapTime = AvarageLapTime;
//                                    break;
//                                default:
//                                    break;
//                            }
//                        }
//                    }
//                    else if (Lines[line].Equals("Ronden"))
//                    {
//                        while (Driver.Ronde(Lines[line + 1])) line++;
//                    }
//                    //else if (Lines[line].Equals("Heat overzicht Beste tijd"))
//                    //{
//                    //    while (Driver.HeatOverzicht(Lines[line + 1])) line++;
//                    //}
//                    //else if (Lines[line].Equals("Records"))
//                    //{
//                    //    while (Records.Parse(Lines[line + 1])) line++;
//                    //}
//                }

//                LiveTiming.Drivers.Add(Driver);
//            }

//            return Me;
//        }

//        public static bool Parse(this Driver Me, string Line, ref string HeatName, ref DateTime DateTime)
//        {
//            if (Me.GetType() == typeof(Driver))
//            {
//                Driver Driver = Me as Driver;

//                //Positie
//                if (Line.Equals("Positie"))
//                {
//                    Me.Position = null;
//                    return true;
//                }
//                //1. EIK#3 - 56
//                else if (Me.Position == null && Me.DriverName == null && Me.KartNumber == null)
//                {
//                    string Prefix = string.Empty;
//                    string Suffix = string.Empty;
//                    if (Line.LastIndexOf('-') > -1) Suffix = Line.Substring(Line.LastIndexOf('-') + 1).TrimStart();
//                    if (Line.IndexOf('.') > -1) Prefix = Line.Remove(Line.IndexOf('.'));
//                    Me.DriverName = Line.Replace(Prefix + ".", string.Empty).Replace(Suffix, string.Empty).Trim();
//                    if (Me.DriverName.LastIndexOf('-') > -1) Me.DriverName = Me.DriverName.Remove(Me.DriverName.LastIndexOf('-'), 1).TrimEnd();
//                    int _Position;
//                    if (int.TryParse(Prefix, out _Position))
//                    {
//                        Me.Position = _Position;
//                        int _KartNumber;
//                        if (int.TryParse(Suffix, out _KartNumber))
//                        {
//                            Me.KartNumber = _KartNumber;
//                            return true;
//                        }
//                    }
//                }
//                else
//                {
//                    string[] Items = Line.Split(':');
//                    //Name:
//                    if (Items[0].Equals("Name"))
//                    {
//                        //this.DriverName = Items[1].Trim();
//                        return true;
//                    }
//                    //Heat: Karten - 2 Uurs Race
//                    else if (Items[0].Equals("Heat"))
//                    {
//                        HeatName = Items[1].Trim();
//                        return true;
//                    }
//                    //Datum: 10-8-2022 18:00:00
//                    else if (Items[0].Equals("Datum"))
//                    {
//                        string DateTimeString = Items[1].Trim();
//                        for (int i = 2; i < Items.Length; i++) DateTimeString += ":" + Items[i];

//                        if (DateTime.TryParseExact(DateTimeString, "d-M-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AllowInnerWhite, out DateTime) == false)
//                        {
//                            if (DateTime.TryParseExact(DateTimeString, "dd-M-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AllowInnerWhite, out DateTime) == false)
//                            {
//                                if (DateTime.TryParseExact(DateTimeString, "d-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AllowInnerWhite, out DateTime) == false)
//                                {
//                                    if (DateTime.TryParseExact(DateTimeString, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AllowInnerWhite, out DateTime) == false)
//                                    {
//                                        return false;
//                                    }
//                                }
//                            }
//                        }
//                        return true;
//                    }
//                    //1
//                    else
//                    {
//                        int _Position;
//                        if (int.TryParse(Items[0], out _Position))
//                        {
//                            if (Me.Position == _Position)
//                            {
//                                return true;
//                            }
//                        }
//                    }
//                }
//            }
//            return false;
//        }

//        public static bool Ronde(this Driver Me, string Line)
//        {
//            string[] Items = Line.Split(' ');
//            if (Items.Length == 2)
//            {
//                int Laps;
//                if (int.TryParse(Items[0], out Laps))
//                {
//                    Me.Laps = Laps;
//                    TimeSpan RondeTijd;
//                    if (TimeSpan.TryParseExact(Items[1], @"s\.fff", CultureInfo.InvariantCulture, out RondeTijd) == false)
//                    {
//                        if (TimeSpan.TryParseExact(Items[1], @"ss\.fff", CultureInfo.InvariantCulture, out RondeTijd) == false)
//                        {
//                            if (TimeSpan.TryParseExact(Items[1], @"m\:ss\.fff", CultureInfo.InvariantCulture, out RondeTijd) == false)
//                            {
//                                if (TimeSpan.TryParseExact(Items[1], @"mm\:ss\.fff", CultureInfo.InvariantCulture, out RondeTijd) == false)
//                                {
//                                    return false;
//                                }
//                            }
//                        }
//                    }
//                    Me.LastLapTime = RondeTijd;
//                    return true;
//                }
//            }
//            return false;
//        }

//        public static bool HeatOverzicht(this Driver Me, string Line)
//        {
//            string[] Items = Line.Split(' ');
//            int Positie;
//            if (int.TryParse(Items[0], out Positie))
//            {
//                Me.Position = Positie;
//                TimeSpan RondeTijd;
//                if (TimeSpan.TryParseExact(Items[Items.Length - 1], @"ss\.fff", CultureInfo.InvariantCulture, out RondeTijd) == false)
//                {
//                    if (TimeSpan.TryParseExact(Items[Items.Length - 1], @"m\:ss\.fff", CultureInfo.InvariantCulture, out RondeTijd) == false)
//                    {
//                        if (TimeSpan.TryParseExact(Items[Items.Length - 1], @"mm\:ss\.fff", CultureInfo.InvariantCulture, out RondeTijd) == false)
//                        {
//                            return false;
//                        }
//                    }
//                }
//                Me.DriverName = Line.Substring(Line.IndexOf(' ')).Remove(Line.LastIndexOf(' '));
//                Me.BestLapTime = RondeTijd;
//                return true;
//            }
//            return false;
//        }
//    }
//}