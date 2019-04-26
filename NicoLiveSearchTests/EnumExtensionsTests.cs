using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NicoLiveSearch.Tests
{
    [TestClass()]
    public class EnumExtensionsTests
    {
        [TestMethod()]
        public void IsDefinedTest()
        {
            SortField.UserId.IsDefined().IsTrue();
            ((SortField)0).IsDefined().IsTrue();
            ((SortField)(-1)).IsDefined().IsFalse();

            Targets.Tags.IsDefined().IsTrue();
            ((Targets)1).IsDefined().IsTrue();
            ((Targets)0).IsDefined().IsFalse();
            ((Targets)(-1)).IsDefined().IsFalse();
            (Targets.Tags | Targets.Title).IsDefined().IsTrue();
            (Targets.Tags | (Targets)1024).IsDefined().IsFalse();
        }

        [TestMethod()]
        public void ToLowerCamelCaseStringTest()
        {
            ResponseFields.ContentId.ToLowerCamelCaseString().Is("contentId");
            ResponseFields.Description.ToLowerCamelCaseString().Is("description");
            ((ResponseFields)1).ToLowerCamelCaseString().Is("contentId");
            AssertEx.Throws<ArgumentOutOfRangeException>(() => ((ResponseFields)0).ToLowerCamelCaseString());

            var e = ResponseFields.ContentId | ResponseFields.Description;
            e.ToLowerCamelCaseString().Is("contentId,description");
        }

        [TestMethod()]
        public void ToSnakeCaseStringTest()
        {
            ResponseFields.ContentId.ToSnakeCaseString().Is("content_id");
            ResponseFields.Description.ToSnakeCaseString().Is("description");
            ((ResponseFields)1).ToSnakeCaseString().Is("content_id");
            AssertEx.Throws<ArgumentOutOfRangeException>(() => ((ResponseFields)0).ToSnakeCaseString());

            var e = ResponseFields.ContentId | ResponseFields.Description;
            e.ToSnakeCaseString().Is("content_id,description");
        }
    }
}
