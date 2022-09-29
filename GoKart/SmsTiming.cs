using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GoKart
{
    public delegate void OnJSONReceived(string Serialized);

    public class BaseConnection
    {
        public string ClientKey { get; set; }
        public string ServiceAddress { get; set; }
        public string AccessToken { get; set; }

        private void ResetClientKey() { ClientKey = null; }
        private void ResetServiceAddress() { ServiceAddress = null; }
        private void ResetAccessToken() { AccessToken = null; }

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
    }

    public class UrlParams
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
                    PropString +=  prop.Name + "=" + prop.GetValue(this) + "&";
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

    static class HttpResponseMessageExtensions
    {
        internal static void WriteRequestToConsole(this HttpResponseMessage response)
        {
            if (response is null)
            {
                return;
            }

            var request = response.RequestMessage;

            Console.WriteLine($"Request URL: {request?.RequestUri} ");
            Console.WriteLine($"Request Method: {request?.Method} ");
            Console.WriteLine($"Request Version: HTTP/{request?.Version}");
            Console.WriteLine($"Status Code: {response.StatusCode}");

            foreach (var Header in response.Headers)
            {
                Console.Write($"{Header.Key}: ");
                foreach (var Value in Header.Value)
                {
                    Console.Write($"{Value}");
                }
                Console.WriteLine();
            }
        }
    }

    public class ConnectionService
    {
        // https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient?redirectedfrom=MSDN

        private OnJSONReceived OnJSONReceived = null;

        private HttpClient httpClient;

        private BaseConnection baseConnection;
        private UrlParams urlParams;
        private string authorizationToken;

        const string constBaseUrl = @"https://backend.sms-timing.com";
        const string constHttps = @"https://";
        const string constApiBestTimes = @"/api/besttimes/";
        const string constResources = @"resources/";
        const string constRecords = @"records/";
        const string constConnectionInfo = @"/api/connectioninfo?type=modules";

        public ConnectionService(OnJSONReceived OnJSONReceived = null)
        {
            this.OnJSONReceived = OnJSONReceived;
            httpClient = new HttpClient();
            urlParams = new UrlParams();
        }

        public void Init(string authorizationToken)
        {
            this.authorizationToken = authorizationToken;
            urlParams.Reset();
            Task task = InitAsync();
        }

        public void Update(string rscId, string scgId, string startDate, string endDate, string maxResults)
        {
            if (rscId != null && scgId != null)
            {
                urlParams.rscId = rscId;
                urlParams.scgId = scgId;
                urlParams.startDate = startDate;
                urlParams.endDate = endDate;
                urlParams.maxResult = maxResults;

                Task task = GetRecordsAsync(httpClient, FindAll(createFullPath(constRecords)));
            }
        }

        private async Task InitAsync()
        {
            // https://backend.sms-timing.com/api/connectioninfo?type=modules
            string url = constBaseUrl + constConnectionInfo;

            if (await GetOptionsAsync(httpClient, url, authorizationToken))
                if (await GetClientParamsAsync(httpClient, url, authorizationToken))
                    await GetResourcesAsync(httpClient, FindAll(createFullPath(constResources)));
        }

        private async Task<bool> GetOptionsAsync(HttpClient client, string url, string authorizationToken)
        {
            using (HttpRequestMessage request = new(HttpMethod.Options, url))
            {
                using (HttpResponseMessage response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode().WriteRequestToConsole();

                    foreach (var header in response.Content.Headers)
                    {
                        Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                    }

                    if (response is { StatusCode: HttpStatusCode.OK })
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private async Task<bool> GetClientParamsAsync(HttpClient client, string url, string authorizationToken)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationToken);

            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                response.EnsureSuccessStatusCode().WriteRequestToConsole();

                var Content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"{Content}\n");

                baseConnection = JsonConvert.DeserializeObject<BaseConnection>(Content);

                if (response is { StatusCode: HttpStatusCode.OK })
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private async Task<bool> GetResourcesAsync(HttpClient client, string url)
        {
            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                response.EnsureSuccessStatusCode().WriteRequestToConsole();

                var Content = await response.Content.ReadAsStringAsync();

                if (response.Content.Headers.ContentType.MediaType.Equals("application/json"))
                {
                    Console.WriteLine($"{Content}\n");

                    object[] resources = JsonConvert.DeserializeObject<object[]>(Content);

                    foreach (var resource in resources)
                    {
                        OnJSONReceived?.Invoke(JsonConvert.SerializeObject(resource));
                    }
                }

                if (response is { StatusCode: HttpStatusCode.OK })
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private async Task<bool> GetRecordsAsync(HttpClient client, string url)
        {
            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                response.EnsureSuccessStatusCode().WriteRequestToConsole();

                var Content = await response.Content.ReadAsStringAsync();

                if (response.Content.Headers.ContentType.MediaType.Equals("application/json"))
                {
                    Console.WriteLine($"{Content}\n");

                    object resources = JsonConvert.DeserializeObject<object>(Content);

                    OnJSONReceived?.Invoke(JsonConvert.SerializeObject(resources));
                }

                if (response is { StatusCode: HttpStatusCode.OK })
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private string createFullPath(string path)
        {
            // https://modules-api10.sms-timing.com/api/besttimes/resources/hezemans
            return constHttps + baseConnection.ServiceAddress + constApiBestTimes + path + baseConnection.ClientKey;
        }

        private string FindAll(string path)
        {
            // https://modules-api10.sms-timing.com/api/besttimes/resources/hezemans?locale=nl&rscId=&accessToken=69cweiiidsjjsssxbdr
            return path + "?" + urlParams.ToString() + "accessToken=" + baseConnection.AccessToken;
        }
    }
}