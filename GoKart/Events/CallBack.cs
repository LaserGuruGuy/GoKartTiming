using System;

namespace GoKart
{
    public partial class MainWindow
    {
        public void OnLiveTiming(string Serialized)
        {
            if (Serialized.Equals("{}"))
            {
                CpbTiming.RaceStatus = "No Races";
#if DEBUG
                Console.WriteLine("No Races");
#endif
            }
            else
            {
#if DEBUG
                Console.WriteLine(Serialized);
#endif
                CpbTiming.RaceStatus = string.Empty;
                CpbTiming.AddLiveTiming(Serialized, true);
            }
        }

        public void OnBestTiming(string Serialized)
        {
#if DEBUG
            Console.WriteLine(Serialized);
#endif
            CpbTiming.AddBestTiming(Serialized);
        }
    }
}