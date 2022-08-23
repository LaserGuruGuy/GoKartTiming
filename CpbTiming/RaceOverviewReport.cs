using System;
using System.Globalization;

namespace CpbTiming
{
    public class RaceOverviewReport
    {
        public PersoonlijkOverzicht PersoonlijkOverzicht { get; set; } = new PersoonlijkOverzicht();
        public RaceGeschiedenis RaceGeschiedenis { get; set; } = new RaceGeschiedenis();
        public Ronden Ronden { get; set; } = new Ronden();
        public HeatOverzicht HeatOverzicht { get; set; } = new HeatOverzicht();
        public Records Records { get; set; } = new Records();

        public RaceOverviewReport(string Page = null)
        {
            Parse(Page);
        }

        public void Parse(string Page)
        {
            string[] Lines = Page.Split('\n');

            for (int line = 0; line < Lines.Length; line++)
            {
                if (Lines[line].Equals("Persoonlijk Overzicht"))
                {
                    while (PersoonlijkOverzicht.Parse(Lines[line + 1])) line++;
                }
                else if (Lines[line].Equals("Uw race geschiedenis (Beste Ronde)"))
                {

                }
                else if (Lines[line].Equals("UW BESTE RONDE"))
                {
                    line++;
                    TimeSpan BesteRondeTijd = new TimeSpan();
                    if (TimeSpan.TryParseExact(Lines[line], @"ss\.fff", CultureInfo.InvariantCulture, out BesteRondeTijd) == false)
                    {
                        if (TimeSpan.TryParseExact(Lines[line], @"m\:ss\.fff", CultureInfo.InvariantCulture, out BesteRondeTijd) == false)
                        {
                            if (TimeSpan.TryParseExact(Lines[line], @"h\:mm\:ss\.fff", CultureInfo.InvariantCulture, out BesteRondeTijd) == false)
                            {
                                break;
                            }
                        }
                    }
                    RaceGeschiedenis.BesteRondeTijd = BesteRondeTijd;
                }
                else if (Lines[line].Equals("Gemiddelde snelheid"))
                {
                    line++;
                    RaceGeschiedenis.GemiddeldeSnelheid = float.Parse(Lines[line], new CultureInfo(CultureInfo.InvariantCulture.Name)
                    {
                        NumberFormat = { NumberDecimalSeparator = "," }
                    });
                }
                else if (Lines[line].Equals("Gemiddelde rondetijd"))
                {
                    line++;
                    TimeSpan GemiddeldeRondetijd = new TimeSpan();
                    if (TimeSpan.TryParseExact(Lines[line], @"s\.fff", CultureInfo.InvariantCulture, out GemiddeldeRondetijd) == false)
                    {
                        if (TimeSpan.TryParseExact(Lines[line], @"ss\.fff", CultureInfo.InvariantCulture, out GemiddeldeRondetijd) == false)
                        {
                            if (TimeSpan.TryParseExact(Lines[line], @"m\:ss\.fff", CultureInfo.InvariantCulture, out GemiddeldeRondetijd) == false)
                            {
                                if (TimeSpan.TryParseExact(Lines[line], @"mm\:ss\.fff", CultureInfo.InvariantCulture, out GemiddeldeRondetijd) == false)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    RaceGeschiedenis.GemiddeldeRondetijd = GemiddeldeRondetijd;
                }
                else if (Lines[line].Equals("Ronden"))
                {
                    while (Ronden.Parse(Lines[line + 1])) line++;
                }
                else if (Lines[line].Equals("Heat overzicht Beste tijd"))
                {
                    while (HeatOverzicht.Parse(Lines[line + 1])) line++;
                }
                else if (Lines[line].Equals("Records"))
                {
                    while (Records.Parse(Lines[line + 1])) line++;
                }
            }
        }
    }
}