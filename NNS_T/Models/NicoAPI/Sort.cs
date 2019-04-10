
namespace NNS_T.Models.NicoAPI
{
    public class Sort
    {
        public SortField Field;
        public bool Ascending;
        public string ToQueryString() => Ascending ? "+" : "-" + Field.ToLowerCamelCaseString();
    }
}
