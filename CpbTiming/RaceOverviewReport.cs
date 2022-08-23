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
                    ;
                }
                else if (Lines[line].Equals("UW BESTE RONDE Gemiddelde snelheid Gemiddelde rondetijd"))
                {
                    line++;
                    string[] Chunk = Lines[line].Split(' ');

                    for (var idx = 0; idx < Chunk.Length; idx++)
                    {
                        switch (idx)
                        {
                            // UW BESTE RONDE
                            case 0:
                                TimeSpan BesteRondeTijd = new TimeSpan();
                                if (TimeSpan.TryParseExact(Chunk[idx], @"ss\.fff", CultureInfo.InvariantCulture, out BesteRondeTijd) == false)
                                {
                                    if (TimeSpan.TryParseExact(Chunk[idx], @"m\:ss\.fff", CultureInfo.InvariantCulture, out BesteRondeTijd) == false)
                                    {
                                        if (TimeSpan.TryParseExact(Chunk[idx], @"h\:mm\:ss\.fff", CultureInfo.InvariantCulture, out BesteRondeTijd) == false)
                                        {
                                            break;
                                        }
                                    }
                                }
                                RaceGeschiedenis.BesteRondeTijd = BesteRondeTijd;
                                break;
                            // Gemiddelde snelheid
                            case 1:
                                RaceGeschiedenis.GemiddeldeSnelheid = float.Parse(Chunk[idx], new CultureInfo(CultureInfo.InvariantCulture.Name)
                                {
                                    NumberFormat = { NumberDecimalSeparator = "," }
                                });
                                break;
                            // Gemiddelde rondetijd
                            case 2:
                                TimeSpan GemiddeldeRondetijd = new TimeSpan();
                                if (TimeSpan.TryParseExact(Chunk[idx], @"s\.fff", CultureInfo.InvariantCulture, out GemiddeldeRondetijd) == false)
                                {
                                    if (TimeSpan.TryParseExact(Chunk[idx], @"ss\.fff", CultureInfo.InvariantCulture, out GemiddeldeRondetijd) == false)
                                    {
                                        if (TimeSpan.TryParseExact(Chunk[idx], @"m\:ss\.fff", CultureInfo.InvariantCulture, out GemiddeldeRondetijd) == false)
                                        {
                                            if (TimeSpan.TryParseExact(Chunk[idx], @"mm\:ss\.fff", CultureInfo.InvariantCulture, out GemiddeldeRondetijd) == false)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                RaceGeschiedenis.GemiddeldeRondetijd = GemiddeldeRondetijd;
                                break;
                            default:
                                break;
                        }
                    }
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