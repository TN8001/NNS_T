using Microsoft.VisualStudio.TestTools.UnitTesting;
using NNS_T.Models.NicoAPI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNS_T.Models.NicoAPI.Tests
{
    [TestClass()]
    public class QueryTests
    {
        [TestMethod()]
        public void FiltersRefactoring()
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



        //[TestMethod()]
        //public void ToStringTest()
        //{
        //    var s = new Query
        //    {
        //        Keyword = "Keyword",
        //        Targets = Targets.Title | Targets.Tags,
        //        Fields = Fields.ContentID | Fields.Title,
        //        Filters = new NameValueCollection { { "liveStatus", "onair" } },
        //        Sort = "-startTime",
        //        Limit = 100,
        //        Context = "appname",
        //    }.ToString();

        //    s.Is("q=Keyword&targets=title,tags&fields=contentId,title&filters[liveStatus][0]=onair&_sort=-startTime&_limit=100&_context=appname");
        //}
    }
}