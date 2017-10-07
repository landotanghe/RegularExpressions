using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RegularExpressionEvaluator.Test
{
    [TestClass]
    public class RegularExpressionTest
    {
        [TestMethod]
        public void OnlyLetters_ExactString_Match()
        {
            var pattern = "abc";
            var text = "abc";

            var regex = new RegularExpression(pattern);

            Assert.IsTrue(regex.IsMatch(text));
        }

        [TestMethod]
        public void OnlyLetters_ContainsString_Match()
        {
            var pattern = "abc";
            var text = "-abc-";

            var regex = new RegularExpression(pattern);

            Assert.IsTrue(regex.IsMatch(text));
        }
    }
}
