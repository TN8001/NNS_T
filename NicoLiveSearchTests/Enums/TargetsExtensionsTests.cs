using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NicoLiveSearch.Tests
{

    [TestClass()]
    public class TargetsExtensionsTests
    {
        [TestMethod()]
        public void ToQueryStringTest()
        {
            AssertEx.Throws<ArgumentOutOfRangeException>(() => ((Targets)0).ToQueryString());
            AssertEx.Throws<ArgumentOutOfRangeException>(() => ((Targets)(-1)).ToQueryString());
            AssertEx.Throws<ArgumentOutOfRangeException>(() => ((Targets)(16)).ToQueryString());

            Targets.TagsExact.ToQueryString().Is("tagsExact");
            (Targets.Title | Targets.Tags).ToQueryString().Is("title,tags");
        }
    }
}
