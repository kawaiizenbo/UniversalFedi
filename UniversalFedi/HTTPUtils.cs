using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Windows.Storage;

namespace UniversalFedi
{
    class HTTPUtils
    {
        static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        // still usin em
        public static async Task<HttpResponseMessage> GETAsync(string URL)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri((string)localSettings.Values["instanceURL"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)localSettings.Values["accessToken"]);
            HttpResponseMessage response = await httpClient.GetAsync(URL);
            httpClient.Dispose();
            return response;
        }

        public static async Task<HttpResponseMessage> GETGenericAsync(string URL)
        {
            HttpClient generic = new HttpClient();
            generic.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)localSettings.Values["accessToken"]);
            HttpResponseMessage response = await generic.GetAsync(new Uri(URL));
            generic.Dispose();
            return response;
        }

        public static async Task<HttpResponseMessage> POSTAsync(string URL, HttpContent data)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri((string)localSettings.Values["instanceURL"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)localSettings.Values["accessToken"]);
            HttpResponseMessage response = await httpClient.PostAsync(URL, data);
            httpClient.Dispose();
            return response;
        }

        public static async Task<HttpResponseMessage> POSTGenericAsync(string URL, HttpContent data)
        {
            HttpClient generic = new HttpClient();
            generic.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)localSettings.Values["accessToken"]);
            HttpResponseMessage response = await generic.PostAsync(new Uri(URL), data);
            generic.Dispose();
            return response;
        }
    }
}
