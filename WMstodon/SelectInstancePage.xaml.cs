using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;

using Newtonsoft.Json.Linq;

using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WMstodon
{
    public sealed partial class SelectInstancePage : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public SelectInstancePage()
        {
            this.InitializeComponent();
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> postopts = new Dictionary<string, string>();
            postopts["client_name"] = "WMStodon";
            postopts["redirect_uris"] = "urn:ietf:wg:oauth:2.0:oob";
            postopts["scopes"] = "read write push";
            postopts["website"] = "https://github.com/kawaiizenbo/WMStodon";
            HttpResponseMessage response = 
                await HTTPUtils.POSTGenericAsync(InstanceURLTextBox.Text + "/api/v1/apps", new FormUrlEncodedContent(postopts));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                JObject appResponseObj = JObject.Parse(await response.Content.ReadAsStringAsync());
                localSettings.Values["instanceURL"] = InstanceURLTextBox.Text;
                Frame.Navigate(typeof(LoginPage), appResponseObj);
            }
            else ErrorTextBlock.Text = "Could not create application on instance:\n" + response.StatusCode;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (localSettings.Values["instanceURL"] != null && localSettings.Values["accessToken"] != null) Frame.Navigate(typeof(MainPage), null);
        }
    }
}
