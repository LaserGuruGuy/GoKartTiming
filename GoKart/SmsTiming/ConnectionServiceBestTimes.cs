﻿using System.Net.Http;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace GoKart.SmsTiming
{
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

    public class ConnectionServiceBestTimes : ConnectionService
    {
        private UrlParamsBestTimes urlParamsBestTimes;

        const string constApiBestTimes = @"/api/besttimes/";
        const string constResources = @"resources/";
        const string constRecords = @"records/";

        public ConnectionServiceBestTimes(OnJSONReceived OnJSONReceived = null)
        {
            this.OnJSONReceived = OnJSONReceived;
            httpClient = new HttpClient();
            urlParamsBestTimes = new UrlParamsBestTimes();
        }

        public void Init(string authorizationToken)
        {
            cancellationTokenSource?.Cancel();
            task?.Wait();

            this.authorizationToken = authorizationToken;
            urlParamsBestTimes.Reset();
            
            cancellationTokenSource = new CancellationTokenSource();
            task = InitAsync();
        }

        private async Task InitAsync()
        {
            string url = constBaseUrl + constConnectionInfo;

            if (await GetOptionsAsync(httpClient, url, cancellationTokenSource.Token))
                if (await GetClientParamsAsync(httpClient, url, authorizationToken, cancellationTokenSource.Token))
                    await GetResourcesAsync(httpClient, FindAll(urlParamsBestTimes, createFullPath(constApiBestTimes + constResources)), cancellationTokenSource.Token);
        }

        public void Update(string rscId, string scgId, string startDate, string endDate, string maxResults)
        {
            if (rscId != null && scgId != null)
            {
                urlParamsBestTimes.rscId = rscId;
                urlParamsBestTimes.scgId = scgId;
                urlParamsBestTimes.startDate = startDate;
                urlParamsBestTimes.endDate = endDate;
                urlParamsBestTimes.maxResult = maxResults;

                Task task = GetRecordsAsync(httpClient, FindAll(urlParamsBestTimes, createFullPath(constApiBestTimes + constRecords)), cancellationTokenSource.Token);
            }
        }
    }
}
