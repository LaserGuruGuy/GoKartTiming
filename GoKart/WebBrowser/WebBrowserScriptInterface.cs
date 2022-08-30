using System;
using System.IO;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace GoKart
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class WebBrowserScriptInterface : IConnectionService
    {
        private MainWindow _MainWindow;

        public WebBrowserScriptInterface(MainWindow MainWindow)
        {
            _MainWindow = MainWindow;
        }

        public string baseUrl { get { return _MainWindow.baseUrl; } }

        public string auth { get { return _MainWindow.auth; } }

        public string path { get; set; } = "/api/livetiming/settings/";

        public string ClientKey { get; set; }

        public string ServiceAddress { get; set; }

        public string AccessToken { get; set; }

        public string url { get { return baseUrl + "/api/connectioninfo?type=modules"; } }

        public string mainPath { get { return "https://" + ServiceAddress + path + ClientKey; } }

        public void onLogMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void onConnectionService(string Serialized)
        {
            JsonConvert.PopulateObject(Serialized, this, new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                ContractResolver = new InterfaceContractResolver(typeof(IConfiguration))
            });
        }

        public void onJSONReceived(string Serialized)
        {
            File.AppendAllText("logfile.json", Serialized + "\n");

            _MainWindow.CpbTiming.Add(Serialized);
        }
    }
}