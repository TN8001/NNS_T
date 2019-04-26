using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NicoLiveSearch.Tests
{
    [TestClass()]
    public class ResponseFieldsExtensionsTests
    {
        [TestMethod()]
        public void ToQueryStringTest()
        {
            AssertEx.Throws<ArgumentOutOfRangeException>(() => ((ResponseFields)0).ToQueryString());
            AssertEx.Throws<ArgumentOutOfRangeException>(() => ((ResponseFields)(-1)).ToQueryString());
            AssertEx.Throws<ArgumentOutOfRangeException>(() => ((ResponseFields)(1 << 19)).ToQueryString());

            ResponseFields.ContentId.ToQueryString().Is("contentId");
            (ResponseFields.ContentId | ResponseFields.Title).ToQueryString().Is("contentId,title");
        }
    }
}
