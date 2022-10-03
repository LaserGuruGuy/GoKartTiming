using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace GoKart.SmsTiming
{
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

    static class HttpResponseMessageExtensions
    {
        internal static void WriteRequestToConsole(this HttpResponseMessage response)
        {
#if DEBUG
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
#endif
        }
    }

    public class ConnectionService
    {
        // https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient?redirectedfrom=MSDN

        protected OnJSONReceived OnJSONReceived;

        protected HttpClient httpClient;

        protected BaseConnection baseConnection;

        protected string authorizationToken;

        protected Task task;
        protected CancellationTokenSource cancellationTokenSource;

        const string constAccessToken = @"accessToken=";
        const string constQuestionMark = @"?";
        const string constBasic = @"Basic";

        const string constApplicationJson = @"application/json";

        const string constHttps = @"https://";

        protected readonly string constBaseUrl = @"https://backend.sms-timing.com";
        protected readonly string constConnectionInfo = @"/api/connectioninfo?type=modules";

        protected async Task<bool> GetOptionsAsync(HttpClient client, string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (HttpRequestMessage request = new(HttpMethod.Options, url))
            {
                using (HttpResponseMessage response = await client.SendAsync(request, cancellationToken))
                {
                    response.EnsureSuccessStatusCode().WriteRequestToConsole();
#if DEBUG
                    foreach (var header in response.Content.Headers)
                    {
                        Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                    }
#endif
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

        protected async Task<bool> GetClientParamsAsync(HttpClient client, string url, string authorizationToken, CancellationToken cancellationToken = default(CancellationToken))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(constBasic, authorizationToken);

            using (HttpResponseMessage response = await client.GetAsync(url, cancellationToken))
            {
                response.EnsureSuccessStatusCode().WriteRequestToConsole();

                var Content = await response.Content.ReadAsStringAsync();

                if (response.Content.Headers.ContentType.MediaType.Equals(constApplicationJson))
                {
#if DEBUG
                    Console.WriteLine($"{Content}\n");
#endif
                    baseConnection = JsonConvert.DeserializeObject<BaseConnection>(Content);
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

        protected async Task<string> GetResourcesAsync(HttpClient client, string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (HttpResponseMessage response = await client.GetAsync(url, cancellationToken))
            {
                response.EnsureSuccessStatusCode().WriteRequestToConsole();

                string Content = await response.Content.ReadAsStringAsync();

                if (response.Content.Headers.ContentType.MediaType.Equals(constApplicationJson))
                {
#if DEBUG
                    Console.WriteLine($"{Content}\n");
#endif
                    try
                    {
                        object[] resources = JsonConvert.DeserializeObject<object[]>(Content);
                        OnJSONReceived?.Invoke(JsonConvert.SerializeObject(resources[0]));
                    }
                    catch
                    {
                    }
                }

                if (response is { StatusCode: HttpStatusCode.OK })
                {
                    return Content;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        protected async Task<bool> GetRecordsAsync(HttpClient client, string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (HttpResponseMessage response = await client.GetAsync(url, cancellationToken))
            {
                response.EnsureSuccessStatusCode().WriteRequestToConsole();

                var Content = await response.Content.ReadAsStringAsync();

                if (response.Content.Headers.ContentType.MediaType.Equals(constApplicationJson))
                {
#if DEBUG
                    Console.WriteLine($"{Content}\n");
#endif
                    OnJSONReceived?.Invoke(Content);
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

        protected string createFullPath(string path)
        {
            // https://modules-api10.sms-timing.com/api/besttimes/resources/hezemans
            // https://modules-api1.sms-timing.com/api/livetiming/settings/circuitparkberghem
            return constHttps + baseConnection.ServiceAddress + path + baseConnection.ClientKey;
        }

        protected string FindAll(Object urlParams, string path)
        {
            // https://modules-api10.sms-timing.com/api/besttimes/resources/hezemans?locale=nl&rscId=&accessToken=69cweiiidsjjsssxbdr
            // https://modules-api1.sms-timing.com/api/livetiming/settings/circuitparkberghem?locale=nl&styleId=&resourceId=&accessToken=00npoqqamipyklkpqpl
            return path + constQuestionMark + urlParams.ToString() + constAccessToken + baseConnection.AccessToken;
        }
    }
}