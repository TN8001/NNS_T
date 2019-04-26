using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NNS_T.Utility.Converters.Tests
{
    [TestClass()]
    public class ShortNunberConverterTests
    {
        [TestMethod()]
        public void ConvertTest()
        {
            var c = new ShortNunberConverter();
            c.Convert(999, null, null, null).Is("999");
            c.Convert(1000, null, null, null).Is("1,000");
            c.Convert(10000, null, null, null).Is("1万");
            c.Convert(10500, null, null, null).Is("1.1万");
            c.Convert(12345678, null, null, null).Is("1,234.6万");
        }
    }
}