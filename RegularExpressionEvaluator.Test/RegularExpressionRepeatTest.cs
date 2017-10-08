﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
