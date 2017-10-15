using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RegularExpressionEvaluator.Test
{
    [TestClass]
    public class RegularExpressionRepeatTest
    {
        [TestMethod]
        public void SingleSymbol_RepeatedAtLeastOnce_Match()
        {
            TestThatRegularExpression.WithPattern("a+")
                .DoesNotMatch("a+")
                .DoesNotMatch(string.Empty)
                .Matches("a")
                .Matches("aa")
                .Matches("aaaaaaaaaaaaaaa");
        }


        [TestMethod]
        public void SingleSymbol_RepeatedAtLeastOnce_PrefixExactlyOnce()
        {
            TestThatRegularExpression.WithPattern("ba+")
                .DoesNotMatch("ba+")
                .DoesNotMatch("b")
                .Matches("ba")
                .Matches("baa")
                .DoesNotMatch("baba")
                .Matches("baaaaaaaaaaaaaaa");
        }

        [TestMethod]
        public void SingleSymbol_RepeatedAtLeastOnce_SuffixExactlyOnce()
        {
            TestThatRegularExpression.WithPattern("a+b")
                .DoesNotMatch("a+b")
                .DoesNotMatch("b")
                .Matches("ab")
                .Matches("aab")
                .DoesNotMatch("abab")
                .Matches("aaaaaaaaaaaaaaab");
        }

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
                .DoesNotMatch("baba")
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
                .DoesNotMatch("abab")
                .Matches("aaaaaaaaaaaaaaab");
        }


        [TestMethod]
        public void SymbolsBetweenParentheses_RepeatedAnyNumberOfTimes()
        {
            TestThatRegularExpression.WithPattern("(ab)*")
                .DoesNotMatch("(ab)*")
                .Matches(string.Empty)
                .Matches("ab")
                .Matches("abab")
                .Matches("abababababababababab");
        }

        [TestMethod]
        public void SymbolsBetweenParentheses_RepeatedAnyNumberOfTimes_PrefixExactlyOnce()
        {
            TestThatRegularExpression.WithPattern("c(ab)*")
                .DoesNotMatch("c(ab)*")
                .Matches("c")
                .Matches("cab")
                .Matches("cabab")
                .Matches("cabababababababababab");
        }


        [TestMethod]
        public void SingleSymbol_RepeatedExactlyThreeTimes()
        {
            TestThatRegularExpression.WithPattern("a{3}")
                .DoesNotMatch("a{3}")
                .DoesNotMatch(string.Empty)
                .DoesNotMatch("a")
                .DoesNotMatch("aaaaaaa")
                .Matches("aaa");
        }


        [TestMethod]
        public void Sequence_RepeatedExactlyThreeTimes()
        {
            TestThatRegularExpression.WithPattern("(ab){3}")
                .DoesNotMatch("(ab){3}")
                .DoesNotMatch(string.Empty)
                .DoesNotMatch("ab")
                .DoesNotMatch("abab")
                .DoesNotMatch("abababab")
                .Matches("ababab");
        }

        [TestMethod]
        public void Sequence_RepeatedMin2_Max5_Times()
        {
            TestThatRegularExpression.WithPattern("(ab){2,5}")
                .DoesNotMatch(string.Empty)
                .DoesNotMatch("ab")
                .Matches("abab")
                .Matches("ababab")
                .Matches("abababab")
                .Matches("ababababab")
                .DoesNotMatch("abababababab");
        }

        [TestMethod]
        public void Sequence_RepeatedMin2_Max5_Times_SuffixExactlyOnce()
        {
            TestThatRegularExpression.WithPattern("(ab){2,5}c")
                .DoesNotMatch(string.Empty)
                .DoesNotMatch("abc")
                .Matches("ababc")
                .Matches("abababc")
                .Matches("ababababc")
                .Matches("abababababc")
                .DoesNotMatch("ababababababc");
        }

        [TestMethod]
        public void Sequence_RepeatedMin2_Max5_Times_PrefixExactlyOnce()
        {
            TestThatRegularExpression.WithPattern("c(ab){2,5}")
                .DoesNotMatch(string.Empty)
                .DoesNotMatch("cab")
                .Matches("cabab")
                .Matches("cababab")
                .Matches("cabababab")
                .Matches("cababababab")
                .DoesNotMatch("cabababababab");
        }

        [TestMethod]
        public void SingleSymbol_RepeatedMin2_Max5_Times()
        {
            TestThatRegularExpression.WithPattern("a{2,5}")
                .DoesNotMatch(string.Empty)
                .DoesNotMatch("a")
                .Matches("aa")
                .Matches("aaa")
                .Matches("aaaa")
                .Matches("aaaaa")
                .DoesNotMatch("aaaaaa");
        }

        [TestMethod]
        public void SingleSymbol_RepeatedMin2_Max5_Times_SuffixExactlyOnce()
        {
            TestThatRegularExpression.WithPattern("a{2,5}b")
                .DoesNotMatch(string.Empty)
                .DoesNotMatch("ab")
                .Matches("aab")
                .Matches("aaab")
                .Matches("aaaab")
                .Matches("aaaaab")
                .DoesNotMatch("aaaaaab");
        }

        [TestMethod]
        public void SingleSymbol_RepeatedMin2_Max5_Times_PrefixExactlyOnce()
        {
            TestThatRegularExpression.WithPattern("ba{2,5}")
                .DoesNotMatch("b")
                .DoesNotMatch("ba")
                .Matches("baa")
                .Matches("baaa")
                .Matches("baaaa")
                .Matches("baaaaa")
                .DoesNotMatch("baaaaaa");
        }
    }
}
