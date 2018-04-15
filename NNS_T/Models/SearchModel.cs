using NNS_T.Models.NicoAPI;
using NNS_T.Utility;
using System.ComponentModel;
using System.Xml.Serialization;

namespace NNS_T.Models
{
    ///<summary>検索設定</summary>
    public class SearchModel : Observable
    {
        ///<summary>検索キーワード</summary>
        public string Query { get => _Query; set => Set(ref _Query, value); }
        private string _Query;

        ///<summary>検索方法</summary>
        [XmlAttribute]
        public Targets Targets { get => _Targets; set => Set(ref _Targets, value); }
        private Targets _Targets;

        ///<summary>検索間隔</summary>
        [XmlAttribute]
        public int IntervalSec
        {
            get => _IntervalSec;
            set
            {
                if(value < 10) value = 10;
                Set(ref _IntervalSec, value);
            }
        }
        private int _IntervalSec;

        ///<summary>レスポンスに使用するフィールド</summary>
        [XmlIgnore]
        public Fields Fields;

        // 否定形はいまいちだが面倒なのでxamlでの使い勝手を優先ｗ
        ///<summary>レスポンスに説明文を含めないかどうか</summary>
        [XmlAttribute]
        public bool UnuseDescription
        {
            get => !Fields.HasFlag(Fields.Description);
            set
            {
                if(UnuseDescription == value) return;
                Fields = value ? Fields & ~Fields.Description
                               : Fields | Fields.Description;
                OnPropertyChanged();
            }
        }

        // コミュ限の放送に鍵アイコンを表示するかどうか
        // アンドキュメントなパラメータで取得できるが規約違反になると思われるので裏設定
        // 自己責任でuser.configに「<Search ShowMemberOnlyIcon="true">」を追加する
        // <Search Targets="Title Description Tags" IntervalSec="60" UnuseDescription="false">
        // ↑こうなっていたとしたら↓こうする
        // <Search Targets="Title Description Tags" IntervalSec="60" UnuseDescription="false" ShowMemberOnlyIcon="true">
        [XmlAttribute, DefaultValue(false)]
        public bool ShowMemberOnlyIcon
        {
            get => Fields.HasFlag(Fields.MemberOnly);
            set
            {
                if(ShowMemberOnlyIcon == value) return;
                Fields = value ? Fields | Fields.MemberOnly
                               : Fields & ~Fields.MemberOnly;
                OnPropertyChanged();
            }
        }

        // コミュ限の放送を検索結果に入れないようにするかどうか
        // アンドキュメントなパラメータで取得できるが規約違反になると思われるので裏設定
        // 自己責任でuser.configに「<Search HideMemberOnly="true">」を追加する
        // <Search Targets="Title Description Tags" IntervalSec="60" UnuseDescription="false" ShowMemberOnlyIcon="true">
        // ↑こうなっていたとしたら↓こうする
        // <Search Targets="Title Description Tags" IntervalSec="60" UnuseDescription="false" ShowMemberOnlyIcon="true" HideMemberOnly="true">
        [XmlAttribute, DefaultValue(false)]
        public bool HideMemberOnly { get => _HideMemberOnly; set => Set(ref _HideMemberOnly, value); }
        private bool _HideMemberOnly;



        public SearchModel()
        {
            Targets = Targets.Title | Targets.Description | Targets.Tags;
            IntervalSec = 30;
            Fields = Fields.LiveAll & ~Fields.Tags & ~Fields.CategoryTags & ~Fields.LiveStatus;
        }
    }
}
