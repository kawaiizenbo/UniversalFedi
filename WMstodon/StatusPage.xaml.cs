using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using Newtonsoft.Json;

using Windows.Data.Html;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace WMstodon
{
    public sealed partial class StatusPage : Page
    {
        Status status = new Status();
        public ObservableCollection<Attachment> Attachments { get; } = new ObservableCollection<Attachment>();

        public StatusPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            string statusURL = e.Parameter.ToString();
            string statusJSON = 
                await (await HTTPUtils.GETAsync("https://" + statusURL.Split('/')[2] + "/api/v1/statuses/" + statusURL.Split('/').Last())).Content.ReadAsStringAsync();
            status = JsonConvert.DeserializeObject<Status>(statusJSON);
            DisplayNameTextBlock.Text = status.account.display_name == "" ? status.account.username : status.account.display_name;
            UsernameTextBlock.Text = $"@{status.account.username}@{status.url.Split('/')[2]}";

            BitmapImage avatar = new BitmapImage();
            avatar.UriSource = new Uri(status.account.avatar_static);
            AvatarImage.Source = avatar;

            ContentTextBlock.Text = HtmlUtilities.ConvertToText(status.content);
            if (status.sensitive) ContentTextBlock.Text = status.spoiler_text + status.content;

            foreach (Attachment a in status.media_attachments)
            {
                Attachments.Add(a);
            }
            ContentTextBlock.UpdateLayout();
            AttachmentImageGrid.Margin = new Thickness(5, 8 + ContentTextBlock.ActualHeight, 5, 0);

            FavoriteButton.Content = $"{(status.favourited ? "Unfavorite" : "Favorite")} ({status.favourites_count})";
            ReblogButton.Content = $"{(status.reblogged ? "Unreblog" : "Reblog")} ({status.reblogs_count})";
            ReplyButton.Content = $"Reply ({status.replies_count})";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!status.favourited)
            {
                await HTTPUtils.POSTAsync($"/api/v1/statuses/{status.id}/favourite", null);
            }
            else
            {
                await HTTPUtils.POSTAsync($"/api/v1/statuses/{status.id}/unfavourite", null);
            }
            FavoriteButton.Content = $"{(status.favourited ? "Unfavorite" : "Favorite")} ({status.favourites_count})";
        }

        private async void ReblogButton_Click(object sender, RoutedEventArgs e)
        {
            if (!status.reblogged)
            {
                await HTTPUtils.POSTAsync($"/api/v1/statuses/{status.id}/reblog", null);
            }
            else
            {
                await HTTPUtils.POSTAsync($"/api/v1/statuses/{status.id}/unreblog", null);
            }
            ReblogButton.Content = $"{(status.reblogged ? "Unreblog" : "Reblog")} ({status.reblogs_count})";
        }

        private void ReplyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AttachmentImage_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (status.media_attachments[0].description != "")
            {
                ContentDialog confirmLogoutDialog = new ContentDialog
                {
                    Title = "Image Description",
                    Content = status.media_attachments[0].description,
                    CloseButtonText = "Dismiss"
                };
            }
        }

        private void Image_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Attachment passedAttachment = (Attachment)AttachmentImageGrid.SelectedItem;
            Frame.Navigate(typeof(ImageViewerPage), passedAttachment);
        }
    }
}
