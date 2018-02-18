using NNS_T.Models;
using NNS_T.Models.NicoAPI;
using NNS_T.Utility;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NNS_T.ViewModels
{
    public class LiveItemViewModel : Observable, IEquatable<LiveItemViewModel>
    {
        private Datum datum;

        ///<summary>生放送ID (lv123456789等)</summary>
        public string LiveID => datum.ContentID;
        ///<summary>タイトル</summary>
        public string Title { get => datum.Title; private set => Set(ref datum.Title, value); }
        ///<summary>説明文 (htmlタグ、改行コード含む)</summary>
        public string Description { get => datum.Description; private set { if(Set(ref datum.Description, value)) OnPropertyChanged(nameof(NonTagDescription)); } }
        //public string Tags => datum.Tags;
        //public string CategoryTags => datum.CategoryTags;
        ///<summary>放送開始日時</summary>
        public DateTime StartTime => datum.StartTime;

        ///<summary>来場者数</summary>
        public int ViewCount { get => datum.ViewCounter; private set => Set(ref datum.ViewCounter, value); }
        ///<summary>コメント数</summary>
        public int CommentCount { get => datum.CommentCounter; private set => Set(ref datum.CommentCounter, value); }
        ///<summary>タイムシフト予約数</summary>
        public int TimeshiftCount { get => datum.ScoreTimeshiftReserved; private set => Set(ref datum.ScoreTimeshiftReserved, value); }
        ///<summary>コミュ限</summary>
        public bool MemberOnly { get => datum.MemberOnly; private set => Set(ref datum.MemberOnly, value); }
        ///<summary>ミュート</summary>
        public bool IsMuted { get => _IsMuted; set => Set(ref _IsMuted, value); }
        private bool _IsMuted;

        ///<summary>説明文からhtmlタグ、改行を除いたもの</summary>
        public string NonTagDescription => Description == null ? null :
            Regex.Replace(Description, @"<.*?>", "").Replace("\r", " ").Replace("\n", " ");

        ///<summary>放送ページURL</summary>
        public string LiveUrl => $"http://nico.ms/{LiveID}";

        ///<summary>サムネイルURL (CommunityIcon優先 なかったらThumbnailUrl)</summary>
        // パラメータを削っているが意味があるかは不明（Imageはある程度キャッシュされるようなのだが。。。
        public string IconUrl => datum.CommunityIcon?.Split('?')[0] ?? datum.ThumbnailUrl?.Split('?')[0];

        ///<summary>放送者種別</summary>
        public ProviderType ProviderType => datum.ThumbnailUrl == null ? ProviderType.User
                                         : datum.CommunityIcon == null ? ProviderType.Official
                                                                       : ProviderType.Channel;

        ///<summary>部屋ID(co1234567やch1234等)</summary>
        public string RoomID
        {
            get
            {
                var m = Regex.Match(IconUrl, "co[0-9]+");
                if(m.Success) return m.Value;

                m = Regex.Match(IconUrl, "ch[0-9]+");
                if(m.Success) return m.Value;

                return null;
            }
        }

        // 追加時アニメーション用 False→Trueで発火 以後使わない
        public bool IsLoaded { get => _IsLoaded; set => Set(ref _IsLoaded, value); }
        private bool _IsLoaded;

        // 取得データが不安定なので何回か待ってから削除する用
        private int DeleteCount;

        public LiveItemViewModel(Datum datum) => this.datum = datum;

        public void Update(LiveItemViewModel item)
        {
            DeleteCount = 0;
            Title = item.Title;
            // 変更可能だと思うが冒頭しか見えないので反映しても負荷が増えるだけで
            // あまりメリットがなさそうなので反映しない
            //Description = item.Description;  
            ViewCount = item.ViewCount;
            CommentCount = item.CommentCount;
            TimeshiftCount = item.TimeshiftCount;
            MemberOnly = item.MemberOnly; // 放送中に変えれるのか？？

            OnPropertyChanged(nameof(StartTime)); // 経過時間更新のためコンバータを作動させる
        }

        public void Delete(ICollection<LiveItemViewModel> items)
        {
            // とりあえず1回待ってみる
            if(DeleteCount > 0) items.Remove(this);
            DeleteCount++;
        }

        // 新しいもの、終わったものの差分を出すのに楽なので。。
        public bool Equals(LiveItemViewModel other) => other == null ? false : (LiveID == other.LiveID);
        public override bool Equals(object obj) => (obj == null || GetType() != obj.GetType()) ? false : Equals((LiveItemViewModel)obj);
        public override int GetHashCode() => LiveID.GetHashCode();
    }
}
