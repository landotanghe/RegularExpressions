using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RegularExpressionEvaluator.Test
{
    public static class TestThatRegularExpression
    {
        public static RegularExpressionToTest WithPattern(string pattern)
        {
            return new RegularExpressionToTest(pattern);
        }
    }

    public class RegularExpressionToTest
    {
        private string _pattern;
        private RegularExpression _regex;

        public RegularExpressionToTest(string pattern)
        {
            _pattern = pattern;
            _regex = RegularExpression.For(pattern);
        }

        public RegularExpressionToTest Matches(string text)
        {

            Assert.IsTrue(_regex.IsMatch(text), $"pattern '{_pattern}' should match '{text}'");

            return this;
        }


        public RegularExpressionToTest DoesNotMatch(string text)
        {
            Assert.IsFalse(_regex.IsMatch(text), $"pattern '{_pattern}' should not match '{text}'");

            return this;
        }
    }
}
