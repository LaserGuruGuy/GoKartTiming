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

        /// <summary>
        /// XMLHttpRequest: LiveTiming, BestTimes
        /// </summary>
        /// <param name="Serialized"></param>
        public void onSuccess(string Serialized)
        {
            Console.WriteLine(Serialized);
        }

        /// <summary>
        /// WebSocket: LiveTiming
        /// </summary>
        public void onError()
        {
            Console.WriteLine("websocket error: no races");
        }

        /// <summary>
        /// HTTP (if WebSocket fails): LiveTiming 
        /// </summary>
        /// <param name="Serialized"></param>
        public void onHTTPMessage(string Serialized)
        {
            OnJSONReceived.Invoke(Serialized);
        }

        /// <summary>
        /// WebSocket: LiveTiming
        /// </summary>
        /// <param name="Serialized"></param>
        public void onMessage(string Serialized)
        {
            OnJSONReceived.Invoke(Serialized);
        }

        /// <summary>
        /// XMLHttpRequest: LiveTiming, BestTimes
        /// </summary>
        /// <param name="Serialized"></param>
        public void onModel(string Serialized)
        {
            OnJSONReceived.Invoke(Serialized);
        }
    }
}