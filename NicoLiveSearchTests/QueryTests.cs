using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NicoLiveSearch.Tests
{
    [TestClass()]
    public class QueryTests
    {
        [TestMethod()]
        public void ToQueryStringTest()
        {
            var q = new Query("a", Targets.TagsExact, SortOrder.StartTimeDesc, "b");
            q.ToQueryString().Is("q=a&targets=tagsExact&_sort=-startTime&_context=b");


            q = new Query("a", Targets.Tags, SortOrder.StartTimeDesc, "b")
            {
                Fields = ResponseFields.ContentId | ResponseFields.Title,
                Filters = new FilterCollection
                {
                    Filters.Tags("t"),
                    Filters.ViewCounter(">",1),
                    Filters.StartTime(DateTime.MinValue,CompOp.GTE),
                    Filters.LiveStatus(LiveStatus.Onair),
                    Filters.MemberOnly(true),
                },
                Offset = 1,
                Limit = 1,
            };
            q.ToQueryString().Is("q=a&targets=tags&fields=contentId,title&filters[tags][0]=t&filters[viewCounter][gt]=1&filters[startTime][gte]=0001-01-01T12:00:00&filters[liveStatus][0]=onair&filters[memberOnly][0]=true&_sort=-startTime&_offset=1&_limit=1&_context=b");


            var s = @"{""type"": ""equal"",""field"": ""viewCounter"",""value"": ""10""}";
            var r = "q=a&targets=tagsExact&jsonFilter=%7B%22type%22%3A%20%22equal%22%2C%22field%22%3A%20%22viewCounter%22%2C%22value%22%3A%20%2210%22%7D&_sort=-startTime&_context=b";
            q = new Query("a", Targets.TagsExact, SortOrder.StartTimeDesc, "b") { JsonFilter = s, };
            q.ToQueryString().Is(r);

            s = @"
{
	""type"": ""equal"",
	""field"": ""viewCounter"",
	""value"": ""10""
}";
            q = new Query("a", Targets.TagsExact, SortOrder.StartTimeDesc, "b") { JsonFilter = s, };
            q.ToQueryString().Is(r);
        }
    }
}