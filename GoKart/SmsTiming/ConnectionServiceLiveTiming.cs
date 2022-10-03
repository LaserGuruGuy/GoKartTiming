using System.Net.Http;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace GoKart.SmsTiming
{
    public delegate void OnJSONReceived(string Serialized);

    public class LiveTimingBase
    {
        public string liveServerKey { get; set; }
        public string liveServerHost { get; set; }
        public int liveServerWsPort { get; set; }
        public int liveServerWssPort { get; set; }
        public int liveServerHttpPort { get; set; }
    }

    public class UrlParamsBestTimes
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string locale { get; set; } = "nl";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string styleId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public string rscId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string scgId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string startDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string endDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string maxResult { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string customCSS { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string scgha { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string scGroup { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string scgHideArray { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string nodeId { get; set; }

        private void Resetlocale() { locale = "nl"; }

        private void ResetstyleId() { styleId = null; }

        private void ResetrscId() { rscId = null; }

        private void ResetscgId() { scgId = null; }

        private void ResetstartDate() { startDate = null; }

        private void ResetendDate() { endDate = null; }

        private void ResetmaxResult() { maxResult = null; }

        private void ResetcustomCSS() { customCSS = null; }

        private void Resetscgha() { scgha = null; }

        private void ResetscGroup() { scGroup = null; }

        private void ResetscgHideArray() { scgHideArray = null; }

        private void ResetnodeId() { nodeId = null; }

        public void Reset()
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(GetType()))
            {
                if (prop.CanResetValue(this))
                {
                    prop.ResetValue(this);
                }
            }
        }

        public override string ToString()
        {
            string PropString = string.Empty;
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(GetType()))
            {
                if (prop.GetValue(this) != null)
                {
                    PropString += prop.Name + "=" + prop.GetValue(this) + "&";
                }
                else
                {
                    foreach (var Attribute in prop.Attributes)
                    {
                        if (Attribute.GetType().Equals(typeof(JsonPropertyAttribute)))
                        {
                            if ((Attribute as JsonPropertyAttribute).NullValueHandling == NullValueHandling.Include)
                            {
                                PropString += prop.Name + "=" + prop.GetValue(this) + "&";
                            }
                        }
                    }
                }
            }
            return PropString;
        }
    }

    public class ConnectionServiceLiveTiming : ConnectionService
    {
        private UrlParamsLiveTiming urlParamsLiveTiming;

        private LiveTimingBase LiveTimingBase;

        const string constApiLiveTiming = @"/api/livetiming/";
        const string constSettings = @"settings/";

        public ConnectionServiceLiveTiming(OnJSONReceived OnJSONReceived = null)
        {
            this.OnJSONReceived = OnJSONReceived;
            httpClient = new HttpClient();
            urlParamsLiveTiming = new UrlParamsLiveTiming();
        }

        public void Init(string authorizationToken)
        {
            cancellationTokenSource?.Cancel();
            task?.Wait();

            urlParamsLiveTiming.Reset();
            this.authorizationToken = authorizationToken;

            cancellationTokenSource = new CancellationTokenSource();
            task = InitAsync();
        }

        private async Task InitAsync()
        {
            string url = constBaseUrl + constConnectionInfo;

            if (await GetOptionsAsync(httpClient, url, cancellationTokenSource.Token))
                if (await GetClientParamsAsync(httpClient, url, authorizationToken, cancellationTokenSource.Token))
                    LiveTimingBase = JsonConvert.DeserializeObject < LiveTimingBase > (await GetResourcesAsync(httpClient, FindAll(urlParamsLiveTiming, createFullPath(constApiLiveTiming + constSettings)), cancellationTokenSource.Token));

            WebSocketService WebSocketService = new WebSocketService(LiveTimingWsUri(LiveTimingBase), LiveTimingBase.liveServerKey, OnJSONReceived, cancellationTokenSource.Token);
        }

        private string LiveTimingWsUri(LiveTimingBase LiveTimingBase)
        {
            return @"ws://" + LiveTimingBase.liveServerHost + ":" + LiveTimingBase.liveServerWsPort;
        }
    }
}