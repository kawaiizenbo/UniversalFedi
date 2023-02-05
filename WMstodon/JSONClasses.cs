﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMstodon
{
    public class Account
    {
        public string id { get; set; }
        public string username { get; set; }
        public string acct { get; set; }
        public string display_name { get; set; }
        public bool locked { get; set; }
        public bool bot { get; set; }
        public bool discoverable { get; set; }
        public bool group { get; set; }
        public DateTime created_at { get; set; }
        public string note { get; set; }
        public string url { get; set; }
        public string avatar { get; set; }
        public string avatar_static { get; set; }
        public string header { get; set; }
        public string header_static { get; set; }
        public int followers_count { get; set; }
        public int following_count { get; set; }
        public int statuses_count { get; set; }
        public object last_status_at { get; set; }
        public object source { get; set; }
        public object[] emojis { get; set; }
        public object[] fields { get; set; }
    }


    public class Attachment
    {
        public string id { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string preview_url { get; set; }
        public string remote_url { get; set; }
        public string text_url { get; set; }
        public AttachmentMeta meta { get; set; }
        public string description { get; set; }
        public string blurhash { get; set; }
    }

    public class AttachmentMeta
    {
        public AttachmentFocus focus { get; set; }
        public AttachmentSizing original { get; set; }
        public AttachmentSizing small { get; set; }
    }

    public class AttachmentFocus
    {
        public float x { get; set; }
        public float y { get; set; }
    }

    public class AttachmentSizing
    {
        public int width { get; set; }
        public int height { get; set; }
        public string size { get; set; }
        public float aspect { get; set; }
    }

    public class Status
    {
        public string id { get; set; }
        public DateTime created_at { get; set; }
        public object in_reply_to_id { get; set; }
        public object in_reply_to_account_id { get; set; }
        public bool sensitive { get; set; }
        public string spoiler_text { get; set; }
        public string visibility { get; set; }
        public string language { get; set; }
        public string uri { get; set; }
        public string url { get; set; }
        public int replies_count { get; set; }
        public int reblogs_count { get; set; }
        public int favourites_count { get; set; }
        public bool favourited { get; set; }
        public bool reblogged { get; set; }
        public bool muted { get; set; }
        public bool bookmarked { get; set; }
        public string content { get; set; }
        public object reblog { get; set; }
        public MApplication application { get; set; }
        public Account account { get; set; }
        public Attachment[] media_attachments { get; set; }
        public object[] mentions { get; set; }
        public object[] tags { get; set; }
        public object[] emojis { get; set; }
        public object card { get; set; }
        public object poll { get; set; }
    }

    public class Feed
    {
        public Status[] statuses { get; set; }
    }

    public class MApplication
    {
        public string name { get; set; }
        public string website { get; set; }
    }
}
