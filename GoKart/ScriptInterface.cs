using System.Security.Permissions;
using System.Runtime.InteropServices;
using System;

namespace GoKart
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class ScriptInterface
    {
        private MainWindow _MainWindow;
        
        public ScriptInterface(MainWindow MainWindow)
        {
            _MainWindow = MainWindow;
        }

        public void onJSONReceived(string received)
        {
            Console.WriteLine(received);
        }

        public void onLogMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}