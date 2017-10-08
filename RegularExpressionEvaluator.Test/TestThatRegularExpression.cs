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

        public RegularExpressionToTest(string pattern)
        {
            _pattern = pattern;
        }

        public RegularExpressionToTest Matches(string text)
        {
            var regex = RegularExpression.For(_pattern);

            Assert.IsTrue(regex.IsMatch(text), $"pattern '{_pattern}' should match '{text}'");

            return this;
        }


        public RegularExpressionToTest DoesNotMatch(string text)
        {
            var regex = RegularExpression.For(_pattern);

            Assert.IsFalse(regex.IsMatch(text), $"pattern '{_pattern}' should not match '{text}'");

            return this;
        }
    }
}
