using System;
using System.Security.Permissions;
using System.Runtime.InteropServices;

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

        public Uri Uri { get; set; }
        public string baseUrl { get; set; } = "https://backend.sms-timing.com";
        public string auth { get; set; }
        public string ClientKey { get; set; }
        public string ServiceAddress { get; set; }
        public string AccessToken { get; set; }

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