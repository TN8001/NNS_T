
namespace NicoLiveSearch
{
    ///<summary>検索フィルター条件</summary>
    public interface IFilter
    {
        ///<summary>比較方法</summary>
        CompOp Op { get; }

        ///<summary>対象フィールド</summary>
        FiltersField Field { get; }

        ///<summary>比較する値の文字列表現</summary>
        string Value { get; }
    }
}
