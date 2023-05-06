using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

using Newtonsoft.Json.Linq;

using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WMstodon
{
    public sealed partial class LoginPage : Page
    {
        JObject passedArgument;
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void OpenAuthPageButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(localSettings.Values["instanceURL"] + "/oauth/authorize" +
                "?client_id=" + passedArgument["client_id"] + "" +
                "&scope=read+write+push" +
                "&redirect_uri=urn:ietf:wg:oauth:2.0:oob" +
                "&response_type=code"
            ));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            passedArgument = (JObject)e.Parameter;
            InstanceTextBlock.Text = (string)localSettings.Values["instanceURL"];
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> postopts = new Dictionary<string, string>();
            postopts["client_id"] = (string)passedArgument["client_id"];
            postopts["client_secret"] = (string)passedArgument["client_secret"];
            postopts["redirect_uri"] = "urn:ietf:wg:oauth:2.0:oob";
            postopts["grant_type"] = "authorization_code";
            postopts["code"] = AuthCodeTextBox.Text;
            postopts["scope"] = "read write push";
            HttpResponseMessage response =
                await HTTPUtils.POSTAsync("/oauth/token", new FormUrlEncodedContent(postopts));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                JObject appResponseObj = JObject.Parse(await response.Content.ReadAsStringAsync());
                localSettings.Values["accessToken"] = (string)appResponseObj["access_token"];
                if ((await HTTPUtils.GETAsync("/api/v1/accounts/verify_credentials")).StatusCode != HttpStatusCode.OK)
                {
                    ErrorTextBlock.Text = "Could not log into service";
                    return;
                }
                Frame.Navigate(typeof(MainPage), appResponseObj);
            }
            else
            {
                ErrorTextBlock.Text = "Could not log into service:\n" + response.StatusCode;
            }
        }
    }
}
