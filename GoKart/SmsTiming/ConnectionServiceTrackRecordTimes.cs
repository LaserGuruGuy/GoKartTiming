using System.Net.Http;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace GoKart.SmsTiming
{
    public class UrlParamsTrackRecordTimes
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
        public string customCSS { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string scGroup { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string scHide { get; set; }

        private void Resetlocale() { locale = "nl"; }

        private void ResetrscId() { rscId = null; }

        private void ResetscgId() { scgId = null; }

        private void ResetscGroup() { scGroup = null; }

        private void ResetscHide() { scHide = null; }

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

    public class ConnectionServiceTrackRecordTimes : ConnectionService
    {
        private UrlParamsTrackRecordTimes urlParamsTrackRecordTimes;

        const string constApiTrackRecord = @"/api/trackrecord/";
        const string constSettings = @"settings/";
        const string constRecords = @"records/";

        public ConnectionServiceTrackRecordTimes(OnJSONReceived OnJSONReceived = null)
        {
            this.OnJSONReceived = OnJSONReceived;
            httpClient = new HttpClient();
            urlParamsTrackRecordTimes = new UrlParamsTrackRecordTimes();
        }

        public void Init(string authorizationToken)
        {
            cancellationTokenSource?.Cancel();
            task?.Wait();

            this.authorizationToken = authorizationToken;
            urlParamsTrackRecordTimes.Reset();
            
            cancellationTokenSource = new CancellationTokenSource();
            task = InitAsync();
        }

        private async Task InitAsync()
        {
            string url = constBaseUrl + constConnectionInfo;

            if (await GetOptionsAsync(httpClient, url, cancellationTokenSource.Token))
                if (await GetClientParamsAsync(httpClient, url, authorizationToken, cancellationTokenSource.Token))
                    await GetResourcesAsync(httpClient, FindAll(urlParamsTrackRecordTimes, createFullPath(constApiTrackRecord + constSettings)), cancellationTokenSource.Token);
        }

        public void Update(string rscId, string scgId, string scGroup, string scHide = null)
        {
            if (rscId != null && scgId != null)
            {
                urlParamsTrackRecordTimes.rscId = rscId;
                urlParamsTrackRecordTimes.scgId = scgId;
                urlParamsTrackRecordTimes.scGroup = scGroup;
                urlParamsTrackRecordTimes.scHide = scHide;

                Task task = GetRecordsAsync(httpClient, FindAll(urlParamsTrackRecordTimes, createFullPath(constApiTrackRecord + constRecords)), cancellationTokenSource.Token);
            }
        }
    }
}
