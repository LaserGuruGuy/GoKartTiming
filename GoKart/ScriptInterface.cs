using System.Security.Permissions;
using System.Runtime.InteropServices;
using System;
using Newtonsoft.Json;

namespace GoKart
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class ScriptInterface : IConnectionService
    {
        private MainWindow _MainWindow;

        public ScriptInterface(MainWindow MainWindow)
        {
            _MainWindow = MainWindow;
        }

        public string baseUrl { get { return _MainWindow.baseUrl; } }

        public string auth { get { return _MainWindow.auth; } }

        public string ClientKey { get; set; }

        public string ServiceAddress { get; set; }

        public string AccessToken { get; set; }

        public string url { get { return baseUrl + "/api/connectioninfo?type=modules"; } }

        public void PolupateConnectionService(string Serialized)
        {
            JsonConvert.PopulateObject(Serialized, this, new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                ContractResolver = new InterfaceContractResolver(typeof(IConfiguration))
            });
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