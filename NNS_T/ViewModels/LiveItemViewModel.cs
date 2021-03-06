﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NicoLiveSearch;
using NNS_T.Utility;

namespace NNS_T.ViewModels
{
    public class LiveItemViewModel : Observable, IEquatable<LiveItemViewModel>
    {
        private readonly Datum datum;

        ///<summary>放送ミュートコマンド</summary>
        public RelayCommand<LiveItemViewModel> ToggleMuteCommand => _ToggleMuteCommand;
        // MainViewModelからインジェクション 雑い
        // staticだと（動くのだが）VS上でエラーが出て目障りなのでちょい変更
        internal static RelayCommand<LiveItemViewModel> _ToggleMuteCommand;


        ///<summary>生放送ID (lv123456789等)</summary>
        public string LiveId => datum.ContentId;

        ///<summary>タイトル</summary>
        public string Title { get => datum.Title; private set => Set(ref datum.Title, value); }

        ///<summary>コミュニティ名</summary>
        public string CommunityText { get => datum.CommunityText; private set => Set(ref datum.CommunityText, value); }

        ///<summary>説明文 (htmlタグ、改行コード含む)</summary>
        public string Description { get => datum.Description; private set { if(Set(ref datum.Description, value)) OnPropertyChanged(nameof(NonTagDescription)); } }

        ///<summary>放送開始日時</summary>
        public DateTime StartTime => datum.StartTime;

        ///<summary>来場者数</summary>
        public int? ViewCount { get => datum.ViewCounter; private set => Set(ref datum.ViewCounter, value); }

        ///<summary>コメント数</summary>
        public int? CommentCount { get => datum.CommentCounter; private set => Set(ref datum.CommentCounter, value); }

        ///<summary>タイムシフト予約数</summary>
        public int TimeshiftCount { get => datum.ScoreTimeshiftReserved; private set => Set(ref datum.ScoreTimeshiftReserved, value); }

        ///<summary>コミュ限</summary>
        public bool MemberOnly { get => datum.MemberOnly; private set => Set(ref datum.MemberOnly, value); }

        ///<summary>ミュート</summary>
        public bool IsMuted { get => _IsMuted; set => Set(ref _IsMuted, value); }
        private bool _IsMuted;

        ///<summary>HD配信かどうか</summary>
        //HD配信の枠制限が撤廃(2018年6月28日)されても「HD配信」タグは残るようなのでこの機会に追加
        public bool IsHD { get; }

        ///<summary>説明文からhtmlタグ、改行を除いたもの</summary>
        public string NonTagDescription => Description == null ? null :
            Regex.Replace(Description, @"<.*?>", "").Replace("\r", " ").Replace("\n", " ");

        ///<summary>放送ページURL</summary>
        public string LiveUrl => datum.LiveUrl;

        ///<summary>サムネイルURL (CommunityIcon優先 なかったらThumbnailUrl)</summary>
        // パラメータを削っているが意味があるかは不明（Imageはある程度キャッシュされるようなのだが。。。
        public string IconUrl => datum.CommunityIcon?.Split('?')[0] ?? datum.ThumbnailUrl?.Split('?')[0];

        ///<summary>放送者種別</summary>
        public ProviderType ProviderType => datum.ProviderType;

        ///<summary>部屋ID(co1234567やch1234等)</summary>
        public string RoomId => datum.RoomId;

        ///<summary>チャンネル or コミュニティページURL</summary>
        public string RoomUrl => datum.RoomUrl;


        // 追加時アニメーション用 False→Trueで発火 以後使わない
        public bool IsLoaded { get => _IsLoaded; set => Set(ref _IsLoaded, value); }
        private bool _IsLoaded;

        // 取得データが不安定なので何回か待ってから削除する用
        public int DeleteCount { get => _DeleteCount; set => Set(ref _DeleteCount, value); }
        private int _DeleteCount;


        public LiveItemViewModel(Datum datum)
        {
            this.datum = datum;
            //途中で変更できそうもないため最初だけ判定
            IsHD = datum.Tags?.Split(' ').Contains("HD配信") ?? false;
        }

        public void Update(LiveItemViewModel item)
        {
            DeleteCount = 0;
            Title = item.Title;

            // 変更の可能性はあるがちょいちょい変わるものでないので反映しない
            //CommunityText = item.CommunityText;  

            // 説明文を取得しない が する に変更された
            if(Description == null && item.Description != null)
            {
                Description = item.Description;
            }

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
        public bool Equals(LiveItemViewModel other) => other == null ? false : (LiveId == other.LiveId);
        public override bool Equals(object obj) => (obj == null || GetType() != obj.GetType()) ? false : Equals((LiveItemViewModel)obj);
        public override int GetHashCode() => LiveId.GetHashCode();
    }
}
