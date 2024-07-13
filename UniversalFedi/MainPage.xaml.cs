using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace UniversalFedi
{
    public sealed partial class MainPage : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        Account myAccount;
        public ObservableCollection<Status> Statuses { get; } = new ObservableCollection<Status>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            string topString = "Loading";
            switch (localSettings.Values["timelineMode"])
            {
                case "home":
                    topString = "Home Timeline";
                    break;
                case "public":
                    topString = "Federated Timeline";
                    break;
                case "public?local=true":
                    topString = "Local Timeline";
                    break;
            }
            TimelineText.Text = topString;

            string accountJSON;

            try
            {
                accountJSON = await (await HTTPUtils.GETAsync("/api/v1/accounts/verify_credentials")).Content.ReadAsStringAsync();
            }
            catch
            {
                Reset();
                return;
            }

            myAccount = JsonConvert.DeserializeObject<Account>(accountJSON);

            DisplayNameTextBlock.Text = $"{myAccount.display_name}";
            UsernameTextBlock.Text = $"@{myAccount.username}@{new Uri((string)localSettings.Values["instanceURL"]).DnsSafeHost}";

            BitmapImage bitmap = new BitmapImage();
            try
            {
                bitmap.UriSource = new Uri(myAccount.avatar_static);
            }
            catch
            {
                Reset();
                return;
            }
            AvatarImage.Source = bitmap;

            await LoadFeed();
        }

        private async Task LoadFeed()
        {
            string feedJSON = ($"{{\"statuses\": {(await HTTPUtils.GETAsync($"/api/v1/timelines/{localSettings.Values["timelineMode"]}")).Content.ReadAsStringAsync().Result}}}");
            Feed feed = new Feed();
            feed = JsonConvert.DeserializeObject<Feed>(feedJSON);
            foreach (Status s in feed.statuses)
            {
                Status status = s;
                string usernameFull = $"@{status.account.username}@{status.account.url.Split('/')[2]}";
                if (status.reblog != null)
                {
                    usernameFull = $"@{status.reblog.account.username}@{status.reblog.account.url.Split('/')[2]}";
                    status = status.reblog;
                    status.additional += $"Reblogged by {s.account.display_name} | ";
                }
                status.content = Windows.Data.Html.HtmlUtilities.ConvertToText(status.content);
                if (status.media_attachments.Length != 0)
                    status.additional += $"{status.media_attachments.Length} " + (status.media_attachments.Length == 1 ? "attachment" : "attachments") + " | ";
                if (status.sensitive)
                {
                    status.content = status.spoiler_text;
                    status.additional += "Tap to view more | ";
                }
                if (status.account.display_name == "")
                    status.account.display_name = status.account.username;
                status.additional += $"{status.favourites_count} Favorites, {status.reblogs_count} Reblogs, {status.replies_count} Replies";
                status.account.acct = usernameFull;
                Statuses.Add(status);
            }
        }

        private async void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog confirmLogoutDialog = new ContentDialog
            {
                Title = "Are you sure you want to log out?",
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Log Out"
            };

            ContentDialogResult result = await confirmLogoutDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                Reset();
            }
        }

        private void FeedListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Status passedStatus = (Status)feedListView.SelectedItem;
            Frame.Navigate(typeof(StatusPage), passedStatus.url);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), null);
        }

        private void NewPostButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewStatusPage), null);
        }

        private void Reset()
        {
            localSettings.Values["instanceURL"] = null;
            localSettings.Values["accessToken"] = null;
            localSettings.Values["timelineMode"] = "home";
            Frame.Navigate(typeof(SelectInstancePage), null);
        }

        private void HomeTimelineButton_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values["timelineMode"] = "home";
            Frame.Navigate(typeof(MainPage), null);
        }

        private void LocalTimelineButton_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values["timelineMode"] = "public?local=true";
            Frame.Navigate(typeof(MainPage), null);
        }

        private void FederatedTimelineButton_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values["timelineMode"] = "public";
            Frame.Navigate(typeof(MainPage), null);
        }
    }
}
