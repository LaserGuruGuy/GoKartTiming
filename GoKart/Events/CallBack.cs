using System;
using System.IO;

namespace GoKart
{
    public partial class MainWindow
    {
        public void OnLiveTiming(string Serialized)
        {
            if (Serialized.Equals("{}"))
            {
#if DEBUG
                Console.WriteLine("No Races");
#endif
            }
            else
            {
#if DEBUG
                Console.WriteLine(Serialized);
                File.AppendAllText("logfile.json", Serialized + "\n");
#endif
                CpbTiming.AddLiveTiming(Serialized);
            }
        }

        public void OnBestTiming(string Serialized)
        {
#if DEBUG
            Console.WriteLine(Serialized);
            File.AppendAllText("logfile.json", Serialized + "\n");
#endif
            CpbTiming.AddBestTiming(Serialized);
        }
    }
}