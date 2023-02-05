using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Newtonsoft.Json;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace WMstodon
{
    public sealed partial class MainPage : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        Account myAccount;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            string accountJSON = (await HTTPUtils.GETAsync("/api/v1/accounts/verify_credentials")).Value;
            myAccount = JsonConvert.DeserializeObject<Account>(accountJSON);

            DisplayNameTextBlock.Text = $"{myAccount.display_name}";
            UsernameTextBlock.Text = $"@{myAccount.username}@{new Uri((string)localSettings.Values["instanceURL"]).DnsSafeHost}";

            BitmapImage bitmap = new BitmapImage();
            bitmap.UriSource = new Uri(myAccount.avatar_static);
            myAvatar.Source = bitmap;

            //await LoadFeed();
        }
    }
}
