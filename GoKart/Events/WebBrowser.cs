using System;
using System.IO;

namespace GoKart
{
    public partial class MainWindow
    {
        public void OnJSONReceived(string Serialized)
        {
#if DEBUG
            Console.WriteLine(Serialized);
            File.AppendAllText("logfile.json", Serialized + "\n");
#endif
            CpbTiming.Add(Serialized);
        }
    }
}
