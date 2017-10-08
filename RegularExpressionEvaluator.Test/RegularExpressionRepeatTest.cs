using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RegularExpressionEvaluator.Test
{
    [TestClass]
    public class RegularExpressionRepeatTest
    {
        [TestMethod]
        public void SingleSymbol_RepeatedAnyNumberOfTimes_Match()
        {
            TestThatRegularExpression.WithPattern("a*")
                .DoesNotMatch("a*")
                .Matches(string.Empty)
                .Matches("a")
                .Matches("aa")
                .Matches("aaaaaaaaaaaaaaa");
        }

        [TestMethod]
        public void SingleSymbol_RepeatedAnyNumberOfTimes_PrefixExactlyOnce()
        {
            TestThatRegularExpression.WithPattern("ba*")
                .DoesNotMatch("ba*")
                .Matches("b")
                .Matches("ba")
                .Matches("baa")
                .Matches("baaaaaaaaaaaaaaa");
        }

        [TestMethod]
        public void SingleSymbol_RepeatedAnyNumberOfTimes_SuffixExactlyOnce()
        {
            TestThatRegularExpression.WithPattern("a*b")
                .DoesNotMatch("a*b")
                .Matches("b")
                .Matches("ab")
                .Matches("aab")
                .Matches("aaaaaaaaaaaaaaab");
        }
    }
}
