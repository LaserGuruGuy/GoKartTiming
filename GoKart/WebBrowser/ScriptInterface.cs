using System.Security.Permissions;
using System.Runtime.InteropServices;
using System;
using Newtonsoft.Json;
using System.IO;
using CpbTiming.SmsTiming;

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

        public void PolupateConnectionService(string Serialized)
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

        public void PopulateFromFile(string FileName)
        {
            if (File.Exists(FileName))
            {
                string[] Lines = File.ReadAllLines(FileName);

                foreach (var Serialized in Lines)
                {
                    _MainWindow.CpbTiming.Add(Serialized);
                }
            }
        }
    }
}