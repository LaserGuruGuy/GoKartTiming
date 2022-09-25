using System;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace GoKart.WebBrowser
{
    public delegate void OnJSONReceived(string Serialized);

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public partial class WebBrowserScriptInterface
    {
        private OnJSONReceived OnJSONReceived = null;

        public WebBrowserScriptInterface(OnJSONReceived OnJSONReceived = null)
        {
            this.OnJSONReceived = OnJSONReceived;
        }

        public Uri Uri { get; set; } = new Uri("pack://siteoforigin:,,,/SmsTiming/LiveTiming.htm");
        //public Uri Uri { get; set; } = new Uri("pack://siteoforigin:,,,/SmsTiming/BestTimes.htm");

        public string auth { get; set; }

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
            OnJSONReceived.Invoke(Serialized);
        }
    }
}