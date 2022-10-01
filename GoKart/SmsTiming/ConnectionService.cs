using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        protected OnJSONReceived OnJSONReceived = null;

        protected HttpClient httpClient;

        protected BaseConnection baseConnection;

        protected string authorizationToken;

        const string constAccessToken = @"accessToken=";
        const string constQuestionMark = @"?";
        const string constBasic = @"Basic";

        const string constApplicationJson = @"application/json";

        const string constHttps = @"https://";

        protected readonly string constBaseUrl = @"https://backend.sms-timing.com";
        protected readonly string constConnectionInfo = @"/api/connectioninfo?type=modules";

        protected async Task<bool> GetOptionsAsync(HttpClient client, string url)
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

        protected async Task<bool> GetClientParamsAsync(HttpClient client, string url, string authorizationToken)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(constBasic, authorizationToken);

            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                response.EnsureSuccessStatusCode().WriteRequestToConsole();

                var Content = await response.Content.ReadAsStringAsync();

                if (response.Content.Headers.ContentType.MediaType.Equals(constApplicationJson))
                {
                    Console.WriteLine($"{Content}\n");

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

        protected async Task<string> GetResourcesAsync(HttpClient client, string url)
        {
            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                response.EnsureSuccessStatusCode().WriteRequestToConsole();

                string Content = await response.Content.ReadAsStringAsync();

                if (response.Content.Headers.ContentType.MediaType.Equals(constApplicationJson))
                {
                    Console.WriteLine($"{Content}\n");
                    if (Content.EndsWith("]"))
                    {
                        object[] resources = JsonConvert.DeserializeObject<object[]>(Content);
                        foreach (var resource in resources)
                        {
                            OnJSONReceived?.Invoke(JsonConvert.SerializeObject(resource));
                        }
                    }
                    else
                    {
                        object resource = JsonConvert.DeserializeObject<object>(Content);
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

        protected async Task<bool> GetRecordsAsync(HttpClient client, string url)
        {
            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                response.EnsureSuccessStatusCode().WriteRequestToConsole();

                var Content = await response.Content.ReadAsStringAsync();

                if (response.Content.Headers.ContentType.MediaType.Equals(constApplicationJson))
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