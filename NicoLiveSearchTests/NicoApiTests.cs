using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;

namespace NicoLiveSearch.Tests
{
    [TestClass()]
    public class NicoApiTests
    {
        [TestMethod()]
        public void GetSearchUrlTest()
        {
            var n = new NicoLiveApi("apiguide");

            AssertEx.Throws<ArgumentNullException>(() => n.GetSearchUrl(null, false));

            var q = new Query("q", Targets.TagsExact, SortOrder.StartTimeDesc, "");
            var s = n.GetSearchUrl(q, false);
            Uri.UnescapeDataString(s).Is("https://live.nicovideo.jp/search?sort=recent&keyword=q&filter=:onair:&kind=tags");

            s = n.GetSearchUrl(q, true);
            Uri.UnescapeDataString(s).Is("https://live.nicovideo.jp/search?sort=recent&keyword=q&filter=:onair:+:channel:+:community:&kind=tags");
        }

        [TestMethod()]
        public async Task GetResponseAsyncTest()
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("*")
                    .WithQueryString("q", "400")
                    .Respond("application/json", "{'meta':{'status':400}}");

            mockHttp.When("*")
                    .WithQueryString("q", "test")
                    .Respond("application/json", "{'meta':{'status':200},'data':[{'title': 'テスト'}]}");


            var n = new NicoLiveApi(mockHttp.ToHttpClient());

            await AssertEx.ThrowsAsync<ArgumentNullException>(() => n.GetResponseAsync(null));

            var q = new Query("400", Targets.Title, SortOrder.StartTimeDesc, "");
            var ex = await AssertEx.ThrowsAsync<NicoApiRequestException>(() => n.GetResponseAsync(q));
            ex.Message.Is("Status:400 不正なパラメータです。");

            q = new Query("test", Targets.Title, SortOrder.StartTimeDesc, "");
            var r = await n.GetResponseAsync(q);
            r.Data[0].Title.Is("テスト");
        }
    }
}


namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    public static partial class AssertEx
    {
        public static async Task<T> ThrowsAsync<T>(Func<Task> task) where T : Exception
        {
            try
            {
                await task();
            }
            catch(Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(T));
                return (T)ex;
            }

            if(typeof(T).Equals(new Exception().GetType()))
                Assert.Fail("Expected exception but no exception was thrown.");
            else
                Assert.Fail($"Expected exception of type {typeof(T)} but no exception was thrown.");

            return null;
        }
    }
}