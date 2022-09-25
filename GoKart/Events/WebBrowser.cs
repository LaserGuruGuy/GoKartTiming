using System.IO;

namespace GoKart
{
    public partial class MainWindow
    {
        public void OnJSONReceived(string Serialized)
        {
            File.AppendAllText("logfile.json", Serialized + "\n");

            CpbTiming.Add(Serialized);
        }
    }
}
