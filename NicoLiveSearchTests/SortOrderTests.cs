using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NicoLiveSearch.Tests
{
    [TestClass()]
    public class SortOrderTests
    {
        [TestMethod()]
        public void ToQueryStringTest()
        {
            new SortOrder(SortField.OpenTime).ToQueryString().Is("-openTime");
            SortOrder.OpenTimeAsc.ToQueryString().Is("+openTime");
        }
    }
}