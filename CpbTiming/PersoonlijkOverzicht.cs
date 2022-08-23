using System;
using System.Globalization;

namespace CpbTiming
{
    public class PersoonlijkOverzicht
    {
        public int? Position { get; private set; }
        public string Team { get; private set; }
        public int? Car { get; private set; }
        public string Driver { get; private set; }
        public string Heat { get; private set; }
        public DateTime Datum { get; private set; }

        public bool Parse(string Line)
        {
            //Positie
            if (Line.Equals("Positie"))
            {
                Position = null;
                return true;
            }
            //1. EIK#3 - 56
            else if (Position == null && Team == null && Car == null)
            {
                string Prefix = string.Empty;
                string Suffix = string.Empty;
                if (Line.LastIndexOf('-') > -1) Suffix = Line.Substring(Line.LastIndexOf('-') + 1).TrimStart();
                if (Line.IndexOf('.') > -1) Prefix = Line.Remove(Line.IndexOf('.'));
                Team = Line.Replace(Prefix + ".", string.Empty).Replace(Suffix, string.Empty).Trim();
                if (Team.LastIndexOf('-') > -1) Team = Team.Remove(Team.LastIndexOf('-'), 1).TrimEnd();
                int _Position;
                if (int.TryParse(Prefix, out _Position))
                {
                    Position = _Position;
                    int _Car;
                    if (int.TryParse(Suffix, out _Car))
                    {
                        Car = _Car;
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
                    this.Driver = Items[1].Trim();
                    return true;
                }
                //Heat: Karten - 2 Uurs Race
                else if (Items[0].Equals("Heat"))
                {
                    this.Heat = Items[1].Trim();
                    return true;
                }
                //Datum: 10-8-2022 18:00:00
                else if (Items[0].Equals("Datum"))
                {
                    string DateTimeString = Items[1].Trim();
                    for (int i = 2; i < Items.Length; i++) DateTimeString += ":" + Items[i];
                    DateTime Datum = new DateTime();

                    if (DateTime.TryParseExact(DateTimeString, "d-M-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AllowInnerWhite, out Datum) == false)
                    {
                        if (DateTime.TryParseExact(DateTimeString, "dd-M-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AllowInnerWhite, out Datum) == false)
                        {
                            if (DateTime.TryParseExact(DateTimeString, "d-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AllowInnerWhite, out Datum) == false)
                            {
                                if (DateTime.TryParseExact(DateTimeString, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AllowInnerWhite, out Datum) == false)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    this.Datum = Datum;
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
    }
}