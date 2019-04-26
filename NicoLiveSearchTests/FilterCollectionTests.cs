using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NicoLiveSearch.Tests
{
    [TestClass()]
    public class FilterCollectionTests
    {
        [TestMethod()]
        public void GetQueryPairsTest()
        {
            var list = new FilterCollection
            {
                Filters.LiveStatus(LiveStatus.Onair),
            }.GetQueryPairs().Select(x => $"{x.Key}={x.Value}");

            var s = string.Join("&", list);
            s.Is("filters[liveStatus][0]=onair");

            list = new FilterCollection
            {
                Filters.Tags("a"),
                Filters.Tags("b"),
                Filters.CommentCounter("<=", 1),
            }.GetQueryPairs().Select(x => $"{x.Key}={x.Value}");

            s = string.Join("&", list);
            s.Is("filters[tags][0]=a&filters[tags][1]=b&filters[commentCounter][lte]=1");

            list = new FilterCollection
            {
                Filters.StartTime(">", DateTime.MinValue),
            }.GetQueryPairs().Select(x => $"{x.Key}={x.Value}");

            s = string.Join("&", list);
            s.Is("filters[startTime][gt]=0001-01-01T12:00:00");

            list = new FilterCollection
            {
                Filters.LiveStatus(LiveStatus.Onair),
                Filters.LiveStatus(LiveStatus.Reserved),
            }.GetQueryPairs().Select(x => $"{x.Key}={x.Value}");

            s = string.Join("&", list);
            s.Is("filters[liveStatus][0]=onair&filters[liveStatus][1]=reserved");
        }
    }
}
