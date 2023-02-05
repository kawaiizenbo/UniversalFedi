using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Windows.Storage;

namespace WMstodon
{
    class HTTPUtils
    {
        static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        // still usin em
        public static async Task<KeyValuePair<HttpStatusCode, string>> GETAsync(string URL)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri((string)localSettings.Values["instanceURL"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)localSettings.Values["accessToken"]);
            HttpResponseMessage response = await httpClient.GetAsync(URL);
            httpClient.Dispose();
            return new KeyValuePair<HttpStatusCode, string>(response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        public static async Task<KeyValuePair<HttpStatusCode, string>> GETGenericAsync(string URL)
        {
            HttpClient generic = new HttpClient();
            generic.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)localSettings.Values["accessToken"]);
            HttpResponseMessage response = await generic.GetAsync(URL);
            generic.Dispose();
            return new KeyValuePair<HttpStatusCode, string>(response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        public static async Task<KeyValuePair<HttpStatusCode, string>> POSTAsync(string URL, HttpContent data)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri((string)localSettings.Values["instanceURL"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)localSettings.Values["accessToken"]);
            HttpResponseMessage response = await httpClient.PostAsync(URL, data);
            httpClient.Dispose();
            return new KeyValuePair<HttpStatusCode, string>(response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        public static async Task<KeyValuePair<HttpStatusCode, string>> POSTGenericAsync(string URL, HttpContent data)
        {
            HttpClient generic = new HttpClient();
            generic.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)localSettings.Values["accessToken"]);
            HttpResponseMessage response = await generic.PostAsync(URL, data);
            generic.Dispose();
            return new KeyValuePair<HttpStatusCode, string>(response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}
