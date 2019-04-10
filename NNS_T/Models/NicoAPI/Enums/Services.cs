using System.Linq;

namespace NNS_T.Models.NicoAPI
{
    // 一応分けたがLiveしか使わない
    ///<summary>ニコニコ機能種別</summary>
    public enum Services
    {
        ///<summary>video 動画</summary>
        Video,

        ///<summary>live 生放送</summary>
        Live,

        ///<summary>illust 静画(イラスト)</summary>
        Illust,

        ///<summary>manga 静画(マンガ)</summary>
        Manga,

        ///<summary>channel チャンネル</summary>
        Channel,

        ///<summary>channelarticle ブロマガ記事(著名人)</summary>
        Channelarticle,

        ///<summary>news ニュース</summary>
        News,

        ///<summary>game ゲーム</summary>
        Game,

        ///<summary>license_search 権利楽曲</summary>
        LicenseSearch,

        ///<summary>mylist_video 動画マイリスト</summary>
        MylistVideo,

        ///<summary>summary まとめ</summary>
        Summary,

        ///<summary>community コミュニティ</summary>
        Community,

        ///<summary>commons コモンズ</summary>
        Commons,
    }
}
