using Microsoft.VisualStudio.TestTools.UnitTesting;
using NNS_T.Models.NicoAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNS_T.Models.NicoAPI.Tests
{
    [TestClass()]
    public class EnumExtensionsTests
    {
        [TestMethod()]
        public void ToLowerCamelCaseStringTest()
        {
            Fields.Description.ToLowerCamelCaseString().Is("description");
            Fields.ContentId.ToLowerCamelCaseString().Is("contentId");
            ((Fields)1).ToLowerCamelCaseString().Is("contentId");
            AssertEx.Throws<ArgumentOutOfRangeException>(() => ((Fields)0).ToLowerCamelCaseString());

            // 組み合わせはエラー
            var e = Fields.Tags | Fields.Title;
            AssertEx.Throws<ArgumentOutOfRangeException>(() => e.ToLowerCamelCaseString());
            // 定義された組み合わせはｏｋ
            Fields.LiveAll.ToLowerCamelCaseString().Is("liveAll");
        }

        [TestMethod()]
        public void ToSnakeCaseStringTest()
        {
            Fields.Description.ToSnakeCaseString().Is("description");
            Fields.ContentId.ToSnakeCaseString().Is("content_id");
            ((Fields)1).ToSnakeCaseString().Is("content_id");
            AssertEx.Throws<ArgumentOutOfRangeException>(() => ((Fields)0).ToSnakeCaseString());

            // 組み合わせはエラー
            var e = Fields.Tags | Fields.Title;
            AssertEx.Throws<ArgumentOutOfRangeException>(() => e.ToSnakeCaseString());
            // 定義された組み合わせはｏｋ
            Fields.LiveAll.ToSnakeCaseString().Is("live_all");
        }
    }
}