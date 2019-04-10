using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;

namespace NNS_T.Utility
{
    internal class Feed
    {
        public Version Version;
        public string Content;
        public string Text;
        public Uri Link;
        public DateTimeOffset? Updated;
        public string Autor;

        public override string ToString() => $"{Updated?.LocalDateTime:d} Ver{Version} {Text}";
    }

    /// <summary>githubから新しいバージョンが出ていないか確認する</summary>
    internal class UpdateChecker
    {
        private readonly string url;
        private readonly Version version;

        /// <summary>githubから新しいバージョンが出ていないか確認する</summary>
        /// <param name="url">github releases.atomのurl</param>
        /// <param name="version">比較するVersion</param>
        public UpdateChecker(string url, Version version)
        {
            this.url = url;
            this.version = version;
        }

        ///<summary>新版があれば「2018/04/21 Ver1.2.0 新バージョン通知機能」といった文字列を返す
        ///すでに最新の場合はstring.Emptyを返す</summary>
        public string GetNewVersionString()
        {
            try
            {
                // どうせリリースミスで何度もappveyorする羽目になるので
                // 満足できる状態になったらgithub releasesに手動でコメントを入れる
                // コメントを入れるまでは最新版扱いをしない
                var feeds = GetFeeds().Where(x => x.Version > version && x.Content != "" && x.Content != "No content.")
                                      .Select(x => x.ToString());

                return string.Join("\n", feeds);
            }
            catch // なにかあっても落とさずにとりあえず動作するように
            {
                return "";
            }
        }

        public IEnumerable<Feed> GetFeeds()
        {
            using(var xr = XmlReader.Create(url))
            {
                var feed = SyndicationFeed.Load(xr);
                return feed.Items.Select(x => new Feed
                {
                    Version = new Version(Regex.Replace(x?.Title?.Text ?? "0.0.0", "[^0-9.]", "")),
                    Content = (x?.Content as TextSyndicationContent)?.Text,
                    Text = Regex.Replace((x?.Content as TextSyndicationContent)?.Text ?? "", "<[^>]*?>", "").Replace("\n", " "),
                    Link = x?.Links.FirstOrDefault()?.Uri,
                    Updated = x?.LastUpdatedTime,
                    Autor = x?.Authors.FirstOrDefault()?.Name
                }).ToArray();
            }
        }
    }
}