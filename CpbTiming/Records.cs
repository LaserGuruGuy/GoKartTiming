using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace CpbTiming
{
    public class Records
    {
        public KeyValuePair<string, RondeTijdDatum> Vandaag { get; set; }
        public KeyValuePair<string, RondeTijdDatum> DezeWeek { get; set; }
        public KeyValuePair<string, RondeTijdDatum> DezeMaand { get; set; }
        public KeyValuePair<string, RondeTijdDatum> DitJaar { get; set; }
        public KeyValuePair<string, RondeTijdDatum> SindsAltijd { get; set; }

        /// <summary>
        /// Vandaag<Team A,<58.634,10-08-202>> "Vandaag Team A 58.634 10-08-202"
        /// DezeWeek<Team B,<58.634,10-08-202>> "Deze week Team B 58.634 10-08-202"
        /// DezeMaand<Team B 2,<58.041,3-08-2022>> "Deze maand Team B 2 58.041 3-08-2022"
        /// DitJaar<Test 15,<02.053,21-05-202>> "Dit jaar Test 15 02.053 21-05-202"
        /// SindsAltijd<Test 15,<02.053,21-05-202>> "Sinds altijd Test 15 02.053 21-05-202"
        /// </summary>
        /// <param name="Line">string to parse to property</param>
        /// <returns></returns>
        public bool Parse(string Line)
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(GetType()))
            {
                string[] Items = null;

                if (Line.StartsWith(prop.Name, true, CultureInfo.InvariantCulture))
                {
                    Items = Line.Replace(prop.Name, string.Empty).Trim().Split(' ');
                }
                else if (Line.Remove(Line.IndexOf(' '), 1).StartsWith(prop.Name, true, CultureInfo.InvariantCulture))
                {
                    Items = Line.Remove(0, prop.Name.Length + 1).Trim().Split(' ');
                }

                if (Items != null)
                {
                    if (Items.Length > 2)
                    {
                        string Key = string.Empty;
                        for (var j = 0; j < Items.Length - 2; j++) Key += Items[j] + ' ';
                        Key.TrimEnd();

                        DateTime RondeDatum = new DateTime();
                        DateTime.TryParseExact(Items[Items.Length - 1], "dd-MM-yyy", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out RondeDatum);

                        TimeSpan RondeTijd = new TimeSpan();
                        if (TimeSpan.TryParseExact(Items[Items.Length - 2], @"ss\.fff", CultureInfo.InvariantCulture, out RondeTijd) == false)
                        {
                            if (TimeSpan.TryParseExact(Items[Items.Length - 2], @"m\:ss\.fff", CultureInfo.InvariantCulture, out RondeTijd) == false)
                            {
                                TimeSpan.TryParseExact(Items[Items.Length - 2], @"mm\:ss\.fff", CultureInfo.InvariantCulture, out RondeTijd);
                            }
                        }

                        KeyValuePair<string, RondeTijdDatum> Value = new KeyValuePair<string, RondeTijdDatum>(Key, new RondeTijdDatum() { RondeTijd = RondeTijd, RondeDatum = RondeDatum });
                        prop.SetValue(this, Value);

                        return true;
                    }
                }
            }

            return false;
        }
    }
}